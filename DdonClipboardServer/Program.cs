using Ddon.Socket;
using Ddon.Socket.Extra;
using DdonTcpServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

    public MyConsoleAppHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        DdonSocketServer<DdonSocketHandler>.CreateServer("127.0.0.1", 5003, _serviceProvider).Start();
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
        if (info.Head.Opcode.Equals(DdonSocketOpcode.Authentication))
        {
            // 客户端要获取在线数量
        }
        else if (info.Head.Opcode.Equals(DdonSocketOpcode.Repost))
        {
            // 客户端要转发文本
        }
        var client = DdonSocketClientConnections<DdonSocketHandler>.GetInstance().GetClient(info.Head.SendClientId);
        Console.WriteLine($"客户端:{info.Head} 数据:{info.Data}");
        if (client is not null)
            await client.SendStringAsync(info.Data);
    };

    public override Action<DdonSocketPackageInfo<byte[]>> FileByteHandler => info => { };

    public override Action<DdonSocketPackageInfo<Stream>> StreamHandler => info => { };
}