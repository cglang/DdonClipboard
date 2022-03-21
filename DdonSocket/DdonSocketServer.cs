using DdonSocket.Extra;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Net;
using System.Net.Sockets;

namespace DdonSocket
{
    public class DdonSocketServer : DdonSocketDefaultHandler
    {
        private readonly IServiceProvider? _services;

        private TcpListener Server { get; set; }

        private DdonSocketServer(string host, int post, IServiceProvider services)
        {
            DdonSocketLogger.InitLogger();
            _services = services;

            var handler = services.GetService<IDdonSocketHandler>() ?? throw new Exception("未找到SocketHandler");
            InitHandler(handler);

            Server = new TcpListener(IPAddress.Parse(host), post);
        }

        private DdonSocketServer(string host, int post, IDdonSocketHandler handler)
        {
            DdonSocketLogger.InitLogger();
            InitHandler(handler);
            Server = new TcpListener(IPAddress.Parse(host), post);
        }

        public static DdonSocketServer CreateServer(string host, int post, IServiceProvider services)
        {
            return new DdonSocketServer(host, post, services);
        }

        public static DdonSocketServer CreateServer(string host, int post, IDdonSocketHandler handler)
        {
            return new DdonSocketServer(host, post, handler);
        }

        private void InitHandler(IDdonSocketHandler handler)
        {
            SetStringContentHandler(handler.StringHandler);
            SetFileByteHandler(handler.FileByteHandler);
            SetStreamHandler(handler.StreamHandler);
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
                    var clientConnection = new DdonSocketConnection(client, _services);
                    DdonSocketClientConnections.GetDdonSocketClientConnectionFactory().Add(clientConnection);

                    clientConnection.SetStringContentHandler(StringHandler);
                    clientConnection.SetFileByteHandler(FileByteHandler);
                    clientConnection.SetStreamHandler(StreamHandler);

                    Log.Information($"接收到客户端:{clientConnection.ClientId}");

                    // 接受来自客户端的数据
                    Task.Run(async () =>
                    {
                        var clientId = await clientConnection.ConsecutiveReadStreamAsync();
                        DdonSocketClientConnections.GetDdonSocketClientConnectionFactory().Remove(clientId);
                        Log.Information($"客户端断开连接:{clientId}");
                    });
                }
            });
        }
    }
}