using Serilog;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace DdonSocket
{
    public class DdonSocketClient : DdonSocketClientBase
    {
        public DdonSocketClient(TcpClient tcpClient) : base(tcpClient) { }

        public DdonSocketClient(string host, int port) : base(host, port) { }


    }
}
