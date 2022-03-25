using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DdonSocket;
using DdonSocket.Extra;
using Microsoft.Extensions.DependencyInjection;

namespace DdonTcpClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var services = new ServiceCollection();

            //var provider = services.BuildServiceProvider();

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