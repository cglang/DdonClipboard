using System.Diagnostics.CodeAnalysis;

namespace DdonClipboardClient
{
    public class ApplicaionConfig
    {
        /// <summary>
        /// 客户端名称
        /// </summary>
        [AllowNull]
        public string ClientName { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 剪切板变化检测间隔时间
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        [AllowNull]
        public string ServerIP { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <returns></returns>
        public static ApplicaionConfig GetDefaultConfig()
        {
            return new()
            {
                ClientName = "客户端名称",
                GroupId = new(),
                Interval = 1000,
                ServerIP = "127.0.0.1",
                ServerPort = 4588
            };
        }
    }
}
