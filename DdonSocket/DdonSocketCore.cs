namespace DdonSocket
{
    public class DdonSocketCore
    {
        /// <summary>
        /// 文本流处理器
        /// </summary>
        protected Action<DdonSocketHeadDto, string>? StringStreamHandler { get; set; }

        /// <summary>
        /// 文件流处理器
        /// </summary>
        protected Action<DdonSocketHeadDto, byte[]>? FileStreamHandler { get; set; }

        /// <summary>
        /// Byte 流处理器
        /// </summary>
        protected Action<DdonSocketHeadDto, Stream>? ByteStreamHandler { get; set; }

        /// <summary>
        /// 处理字符串
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public DdonSocketCore StringContentHandler(Action<DdonSocketHeadDto, string>? handler)
        {
            if (handler is not null) StringStreamHandler = handler;
            return this;
        }

        /// <summary>
        /// 处理小文件，直接获取文件bytes
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public DdonSocketCore FileByteHandler(Action<DdonSocketHeadDto, byte[]>? handler)
        {
            if (handler is not null) FileStreamHandler = handler;
            return this;
        }

        /// <summary>
        /// 直接处理流，主要用于大文件传输
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public DdonSocketCore StreamHandler(Action<DdonSocketHeadDto, Stream>? handler)
        {
            if (handler is not null) ByteStreamHandler = handler;
            return this;
        }
    }
}
