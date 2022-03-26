
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

            //Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

            //await Host.CreateDefaultBuilder(args).ConfigureServices(services =>
            //{
            //    services.AddHostedService<MyConsoleAppHostedService>();
            //}).RunConsoleAsync();

            //        await Host.CreateDefaultBuilder(args)
            //.ConfigureServices(services =>
            //{
            //    services.AddHostedService<MyConsoleAppHostedService>();
            //})
            //.RunConsoleAsync();


            // 从appsettings.json文件中读入日志的配置信息
            //var configuration = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            //var services = new ServiceCollection();
            //services.AddLogging(x =>
            //{
            //    //x.SetMinimumLevel(LogLevel.Debug);
            //    x.AddConfiguration(configuration);
            //    x.AddConsole(); // 将日志输出到控制台
            //});
            //var provider = services.BuildServiceProvider();

            //var log = provider.GetRequiredService<ILogger<Program>>();
            //log.LogInformation("testi");
            //log.LogDebug("testd");
            //log.LogWarning("w");
            //log.LogError("e");

            var ddonTcpClient = new DdonSocketClient<DdonSocketHandler>("127.0.0.1", 5003);

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