using DdonSocket;
using DdonSocket.Extra;

namespace WebApplication1
{
    public class TestDdonSocketHandler : IDdonSocketHandler
    {
        public override Action<IServiceProvider?, DdonSocketHeadDto, string> StringHandler => async (service, head, data) =>
        {
            if (head.Opcode.Equals(DdonSocketOpcode.Authentication))
            {
                // 客户端要获取在线数量
            }
            else if (head.Opcode.Equals(DdonSocketOpcode.Repost))
            {
                // 客户端要转发文本
            }
            var client = DdonSocketClientConnectionFactory.GetDdonTcpClient(head.SendClientId);
            Console.WriteLine($"客户端:{head} 数据:{data}");
            if (client is not null)
                await client.SendStringAsync(data);
        };

        public override Action<IServiceProvider?, DdonSocketHeadDto, byte[]> FileByteHandler => (a, b, c) => { };

        public override Action<IServiceProvider?, DdonSocketHeadDto, Stream> StreamHandler => (a, b, c) => { };
    }
}
