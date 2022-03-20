using DdonSocket.Extra;

namespace DdonSocket
{
    public abstract class IDdonSocketHandler
    {
        /// <summary>
        /// 文本流处理器
        /// </summary>
        public abstract Action<IServiceProvider?, DdonSocketHeadDto, string> StringHandler { get; }

        /// <summary>
        /// 文件流处理器
        /// </summary>
        public abstract Action<IServiceProvider?, DdonSocketHeadDto, byte[]> FileByteHandler { get; }

        /// <summary>
        /// Byte 流处理器
        /// </summary>
        public abstract Action<IServiceProvider?, DdonSocketHeadDto, Stream> StreamHandler { get; }
    }
}
