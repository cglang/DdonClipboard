using System.Net.Sockets;

namespace DdonSocket
{
    public class DdonSocketConnection : DdonSocketClient
    {
        public DdonSocketConnection(TcpClient tcpClient, IServiceProvider? service) : base(tcpClient, service) { }

        public new async Task<Guid> ConsecutiveReadStreamAsync() => await base.ConsecutiveReadStreamAsync();
    }
}
