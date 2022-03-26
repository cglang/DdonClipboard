using Ddon.KeyValueStorage;
using Ddon.Socket;
using Ddon.Socket.Extra;
using DdonClipboardServer;
using DdonTcpServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<MyConsoleAppHostedService>();
    })
    .CreateApplication<ServerModule>()
    .RunConsoleAsync();


class MyConsoleAppHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MyConsoleAppHostedService> _logger;

    public MyConsoleAppHostedService(IServiceProvider serviceProvider, ILogger<MyConsoleAppHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // 配置文件读取和初始化
        var configKv = DdonKvStorageFactory<ApplicaionConfig>.GetInstance(slice: "config");
        var config = await configKv.GetValueAsync("config");
        if (config is null)
        {
            await configKv.SetValueAsync("config", ApplicaionConfig.GetDefaultConfig());
            _logger.LogError("请修改配置文件");
            return;
        }

        DdonSocketServer<DdonSocketHandler>.CreateServer(config.Ip, config.Port, _serviceProvider).Start();
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}

class DdonSocketHandler : DdonSocketHandlerCore
{
    public override Action<DdonSocketPackageInfo<string>> StringHandler => async info =>
    {
        var logger = info.ServiceProvider.GetRequiredService<ILogger<DdonSocketHandler>>();
        logger.LogInformation($"客户端:{info.Head} 消息:{info.Data}");

        if (info.Head.OpCode.Equals(DdonSocketOpcode.Authentication))
        {
            var client = DdonSocketClientConnections<DdonSocketHandler>.GetInstance().GetClient(info.Head.ClientId);
            if (client != null) client.GroupId = info.Head.GroupId;
        }
        else if (info.Head.OpCode.Equals(DdonSocketOpcode.Repost))
        {
            if (info.Head.SendGroup != default)
            {
                var clients = DdonSocketClientConnections<DdonSocketHandler>.GetInstance().GetClients(info.Head.SendGroup);
                if (clients == null) return;
                Parallel.ForEach(clients, async client =>
                {
                    if (client.ClientId != info.Head.ClientId)
                        await client.SendStringAsync(DdonSocketOpcode.Repost, info.Data, client.ClientId);
                });
            }
            else if (info.Head.SendClient != default)
            {
                var client = DdonSocketClientConnections<DdonSocketHandler>.GetInstance().GetClient(info.Head.SendClient);
                if (client == null) return;
                await client.SendStringAsync(DdonSocketOpcode.Repost, info.Data, client.ClientId);
            }
        }
    };

    public override Action<DdonSocketPackageInfo<byte[]>> FileByteHandler => info => { };

    public override Action<DdonSocketPackageInfo<Stream>> StreamHandler => info => { };
}