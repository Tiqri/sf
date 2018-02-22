using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blissmo.API.Handlers;
using Blissmo.API.Mapper;
using Blissmo.API.Model;
using Blissmo.Helper.MessageBrokerProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blissmo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //var builder = new ConfigurationBuilder()
            //        .SetBasePath(env.ContentRootPath)
            //        .AddInMemoryCollection()
            //        .AddJsonFile("appsettings.json", false, true);

            //Configuration = builder.Build();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            //var endpoint = Configuration.GetSection("MessageBroker:EndPoint");
            //services.Configure<MessageBroker>(Configuration.GetSection("MessageBroker"));
            services.AddSingleton<IEventHandler, Handlers.EventHandler>();
            services.AddTransient<IMessageBroker, ServiceBusMessageBroker>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            AutoMapperConfiguration.Configure();
            app.UseMvc();

            app.UseServiceBusListener();
        }
    }
}
