using karciSinav.Models.data;
using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(karciSinav.Startup))]
namespace karciSinav
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SeedData.Initialize();
        }
    }
}