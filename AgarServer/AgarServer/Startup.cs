using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using Ninject;

[assembly: OwinStartup(typeof(AgarServer.Startup))]

namespace AgarServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            //init Game engine
            var instance = GameEngine.Instance;
        }
    }
}
