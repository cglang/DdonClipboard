namespace DdonTcpClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var ddonTcpClient = new DdonSocket.DdonSocketClient("127.0.0.1", 5003);

            ddonTcpClient.SetStringContentHandler((info) =>
            {
                Console.WriteLine($"接收到来自服务端的数据:{info.Data}");
            });

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
}