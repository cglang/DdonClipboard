using DdonSocket.Extra;
using Serilog;
using System.Net;
using System.Net.Sockets;

namespace DdonSocket
{
    public class DdonSocketServer : DdonSocketCore
    {
        private readonly Dictionary<Guid, DdonSocketClient> Pairs = new();

        private TcpListener Server { get; set; }

        private DdonSocketServer(string host, int post)
        {
            DdonSocketLogger.InitLogger();
            Server = new TcpListener(IPAddress.Parse(host), post);
        }

        public static DdonSocketServer CreateServer(string host, int post)
        {
            return new DdonSocketServer(host, post);
        }

        public DdonSocketClient? GetDdonTcpClient(Guid clientId)
        {
            return Pairs.ContainsKey(clientId) ? Pairs[clientId] : null;
        }

        public void Start()
        {
            Log.Information("监听客户端接入请求...");
            Server.Start();

            while (true)
            {
                var client = Server.AcceptTcpClient();
                var connection = new DdonSocketConnection(client);
                Pairs.Add(connection.ClientId, connection);

                connection.StringContentHandler(StringStreamHandler);
                connection.StreamHandler(ByteStreamHandler);
                connection.FileByteHandler(FileStreamHandler);

                Log.Information($"接收到客户端:{connection.ClientId}");

                // 接受来自客户端的数据
                Task.Run(async () =>
                {
                    var clientId = await connection.ConsecutiveReadStreamAsync();
                    Pairs.Remove(clientId);
                    Log.Information($"客户端断开连接:{clientId}");
                });
            }
        }


        public void Stop()
        {
            // todo: 释放掉现有链接 停止监听
            Server.Stop();
        }
    }
}