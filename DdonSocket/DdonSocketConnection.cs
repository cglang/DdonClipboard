using System.Net.Sockets;

namespace DdonSocket
{
    public class DdonSocketConnection : DdonSocketClient
    {
        public DdonSocketConnection(TcpClient tcpClient) : base(tcpClient) { }

        public new async Task<Guid> ConsecutiveReadStreamAsync() => await base.ConsecutiveReadStreamAsync();
    }
}
