using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WalksInNature.Test
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);         

        }
    }
}
