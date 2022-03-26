using Ddon.Core;
using Ddon.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DdonTcpServer
{
    public class ServerModule : Module
    {
        public override void Load(IServiceCollection services, IConfiguration configuration)
        {
            Load<SerilogModule>(services, configuration);
        }
    }
}
