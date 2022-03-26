using Ddon.Serilog;
using Ddon.Socket;
using Ddon.Socket.Extra;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DdonTcpClient
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

        public MyConsoleAppHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var ddonTcpClient = new DdonSocketClient<DdonSocketHandler>("127.0.0.1", 5003, _services);

            ddonTcpClient.StartRead();

            Console.WriteLine($"被分配Id:{ddonTcpClient.ClientId}");

            while (true)
            {
                Console.Write("输入目标客户端Id:");
                string sendClientIdText = Console.ReadLine() ?? string.Empty;
                var sendClientId = Guid.Parse(sendClientIdText);
                var sendData = Console.ReadLine();
                await ddonTcpClient.SendStringAsync($"{sendData}", sendClientId);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }

    class DdonSocketHandler : DdonSocketHandlerCore
    {
        public override Action<DdonSocketPackageInfo<string>> StringHandler => info =>
        {
            Console.WriteLine($"接收到来自服务端的数据:{info.Data}");
        };

        public override Action<DdonSocketPackageInfo<byte[]>> FileByteHandler => info => { };

        public override Action<DdonSocketPackageInfo<Stream>> StreamHandler => info => { };
    }
}