using Serilog;

namespace DdonSocket
{
    public class DdonSocketLogger
    {
        private static object? obj = null;

        public static void InitLogger()
        {
            if (obj is null)
            {
                string Logformat = @"{Timestamp:yyyy-MM-dd HH:mm-dd }[{Level:u3}] {Message:lj}{NewLine}";
                Log.Logger = new LoggerConfiguration()
                    //设置最低等级
                    .MinimumLevel.Debug()
                    //将事件发送到控制台并展示
                    .WriteTo.Console(outputTemplate: Logformat)
                    .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "Logs/logs.txt"), rollingInterval: RollingInterval.Day)
                    .CreateLogger();
                obj = new object();
            }
        }
    }
}
