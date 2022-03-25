using DdonSocket.Extra;
using Serilog;
using System.Net;
using System.Net.Sockets;

namespace DdonSocket
{
    public class DdonSocketServer<TDdonSocketHandler> where TDdonSocketHandler : DdonSocketHandlerCore, new()
    {
        private readonly IServiceProvider? _services;

        private TcpListener Server { get; set; }

        private DdonSocketServer(string host, int post, IServiceProvider services)
        {
            DdonSocketLogger.InitLogger();
            _services = services;

            Server = new TcpListener(IPAddress.Parse(host), post);
        }

        private DdonSocketServer(string host, int post)
        {
            DdonSocketLogger.InitLogger();
            Server = new TcpListener(IPAddress.Parse(host), post);
        }

        public static DdonSocketServer<TDdonSocketHandler> CreateServer(string host, int post, IServiceProvider services)
        {
            return new DdonSocketServer<TDdonSocketHandler>(host, post, services);
        }

        public static DdonSocketServer<TDdonSocketHandler> CreateServer(string host, int post)
        {
            return new DdonSocketServer<TDdonSocketHandler>(host, post);
        }

        public void Start()
        {
            Log.Information("监听客户端接入请求...");
            Server.Start();

            Task.Run(() =>
            {
                while (true)
                {
                    var client = Server.AcceptTcpClient();
                    var clientConnection = new DdonSocketConnection<TDdonSocketHandler>(client, _services);
                    DdonSocketClientConnections<TDdonSocketHandler>.GetDdonSocketClientConnectionFactory().Add(clientConnection);

                    Log.Information($"接收到客户端:{clientConnection.ClientId}");

                    // 接受来自客户端的数据
                    Task.Run(async () =>
                    {
                        var clientId = await clientConnection.ConsecutiveReadStreamAsync();
                        DdonSocketClientConnections<TDdonSocketHandler>.GetDdonSocketClientConnectionFactory().Remove(clientId);
                        Log.Information($"客户端断开连接:{clientId}");
                    });
                }
            });
        }
    }
}