﻿using DdonSocket.Extra;
using System.Diagnostics.CodeAnalysis;

namespace DdonSocket
{
    public class DdonSocketDefaultHandler : IDdonSocketHandler
    {
        public override Action<IServiceProvider?, DdonSocketHeadDto, string> StringHandler => StringStreamHandler;

        public override Action<IServiceProvider?, DdonSocketHeadDto, byte[]> FileByteHandler => FileStreamHandler;

        public override Action<IServiceProvider?, DdonSocketHeadDto, Stream> StreamHandler => ByteStreamHandler;

        /// <summary>
        /// 文本流处理器
        /// </summary>
        [AllowNull]
        private Action<IServiceProvider?, DdonSocketHeadDto, string> StringStreamHandler { get; set; }

        /// <summary>
        /// 文件流处理器
        /// </summary>
        [AllowNull]
        private Action<IServiceProvider?, DdonSocketHeadDto, byte[]> FileStreamHandler { get; set; }

        /// <summary>
        /// Byte 流处理器
        /// </summary>
        [AllowNull]
        private Action<IServiceProvider?, DdonSocketHeadDto, Stream> ByteStreamHandler { get; set; }

        /// <summary>
        /// 处理字符串
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public DdonSocketDefaultHandler SetStringContentHandler(Action<IServiceProvider?, DdonSocketHeadDto, string>? handler)
        {
            StringStreamHandler = handler;
            return this;
        }

        /// <summary>
        /// 处理小文件，直接获取文件bytes
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public DdonSocketDefaultHandler SetFileByteHandler(Action<IServiceProvider?, DdonSocketHeadDto, byte[]>? handler)
        {
            FileStreamHandler = handler;
            return this;
        }

        /// <summary>
        /// 直接处理流，主要用于大文件传输
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public DdonSocketDefaultHandler SetStreamHandler(Action<IServiceProvider?, DdonSocketHeadDto, Stream>? handler)
        {
            ByteStreamHandler = handler;
            return this;
        }
    }
}