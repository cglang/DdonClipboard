using DdonSocket;
using DdonSocket.Extra;

namespace WebApplication1
{
    public class TestDdonSocketHandler : IDdonSocketHandler
    {
        public override Action<DdonSocketPackageInfo<string>> StringHandler => async (info) =>
        {
            if (info.Head.Opcode.Equals(DdonSocketOpcode.Authentication))
            {
                // 客户端要获取在线数量
            }
            else if (info.Head.Opcode.Equals(DdonSocketOpcode.Repost))
            {
                // 客户端要转发文本
            }
            var client = DdonSocketClientConnectionFactory.GetDdonSocketClientConnectionFactory().GetClient(info.Head.SendClientId);
            Console.WriteLine($"客户端:{info.Head} 数据:{info.Data}");
            if (client is not null)
                await client.SendStringAsync(info.Data);
        };

        public override Action<DdonSocketPackageInfo<byte[]>> FileByteHandler => (info) => { };

        public override Action<DdonSocketPackageInfo<Stream>> StreamHandler => (info) => { };
    }
}
