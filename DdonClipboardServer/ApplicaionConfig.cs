using System.Diagnostics.CodeAnalysis;

namespace DdonClipboardServer
{
    public class ApplicaionConfig
    {
        /// <summary>
        /// 服务器IP
        /// </summary>
        [AllowNull]
        public string Ip { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <returns></returns>
        public static ApplicaionConfig GetDefaultConfig()
        {
            return new()
            {
                Ip = "127.0.0.1",
                Port = 4588
            };
        }
    }
}
