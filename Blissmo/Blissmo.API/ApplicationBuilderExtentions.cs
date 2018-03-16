using Blissmo.API.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Blissmo.API
{
    public static class ApplicationBuilderExtentions
    {
        public static IEventHandler EventHandler { get; set; }

        public static IApplicationBuilder UseServiceBusListener(this IApplicationBuilder app)
        {
            try
            {
                EventHandler = app.ApplicationServices.GetService<IEventHandler>();

                var appLifeTime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

                appLifeTime.ApplicationStarted.Register(OnStarted);

                appLifeTime.ApplicationStopping.Register(OnStopping);

                return app;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }       
        }

        private static void OnStarted()
        {
            EventHandler.Register();
        }

        private static void OnStopping()
        {
            EventHandler.Deregister();
        }
    }
}
