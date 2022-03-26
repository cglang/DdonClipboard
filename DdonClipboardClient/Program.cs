using Ddon.KeyValueStorage;
using Ddon.Serilog;
using Ddon.Socket;
using Ddon.Socket.Extra;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TextCopy;
using STimer = System.Timers.Timer;

namespace DdonClipboardClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<MyConsoleAppHostedService>();
                })
                .CreateApplication<SerilogModule>()
                .RunConsoleAsync();
        }
    }

    class MyConsoleAppHostedService : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<MyConsoleAppHostedService> _logger;

        public MyConsoleAppHostedService(IServiceProvider services, ILogger<MyConsoleAppHostedService> logger)
        {
            _services = services;
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

            // 连接服务器
            var ddonTcpClient = new DdonSocketClient<DdonSocketHandler>(config.ServerIP, config.ServerPort, _services);
            ddonTcpClient.StartRead();
            _logger.LogInformation($"被分配Id:{ddonTcpClient.ClientId}");

            await ddonTcpClient.SendAuthenticationInfoAsync(config.GroupId);

            // 检测剪切板变化
            STimer timer = new() { Enabled = true, Interval = 1000 };
            timer.Elapsed += async (x, y) =>
            {
                var text = await ClipboardService.GetTextAsync();
                if (!string.IsNullOrEmpty(text) && ClipboardText.T != text)
                {
                    _logger.LogInformation($"剪切板变化:{text}");
                    ClipboardText.T = text ?? string.Empty;
                    await ddonTcpClient.SendStringAsync(DdonSocketOpcode.Repost, $"{ClipboardText.T}", sendGroupId: config.GroupId);
                }
            };
            timer.Start();
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
            logger.LogInformation($"接收到剪切板同步:{info.Data}");
            ClipboardText.T = info.Data;
            await ClipboardService.SetTextAsync(info.Data);
        };

        public override Action<DdonSocketPackageInfo<byte[]>> FileByteHandler => info => { };

        public override Action<DdonSocketPackageInfo<Stream>> StreamHandler => info => { };
    }
}