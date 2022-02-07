using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JoshDevenyi_PokemonPassionProject.Startup))]
namespace JoshDevenyi_PokemonPassionProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
