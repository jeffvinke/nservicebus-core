using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBusCore;
using Castle.Windsor.MsDependencyInjection;
using Castle.Windsor.Installer;

namespace NServiceBus_Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static IWindsorContainer Container { get; } = new WindsorContainer();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Container.Install(FromAssembly.InThisApplication(System.Reflection.Assembly.GetExecutingAssembly()));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            var applicationSettings = new ApplicationSettings();
            Configuration.GetSection("ApplicationSettings").Bind(applicationSettings);

            //services.AddDbContext<>()

            var endpointConfig = new EndpointConfiguration("NserviceBusCore");
            var transport = endpointConfig.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            //routing.RouteToEndpoint(
            //    assembly: typeof(NServiceBusCore.Messages).Assembly,
            //    destination: "NserviceBusCore");


            //endpointConfig.SendFailedMessagesTo("");
            //endpointConfig.AuditProcessedMessagesTo("");
            //endpointConfig.LicensePath("");

            endpointConfig.Conventions()
                .DefiningCommandsAs(x => x.Namespace != null && x.Namespace == "NServiceBusCore.Messages.Commands")
                .DefiningEventsAs(x => x.Namespace != null && x.Namespace == "NServiceBusCore.Messages.Events")
                .DefiningMessagesAs(x => x.Namespace != null && x.Namespace == "NServiceBusCore.Messages.Messages");

            endpointConfig.UseContainer<WindsorBuilder>(c => c.ExistingContainer(Container));            
            endpointConfig.EnableInstallers();

            var endpointInstance = Endpoint.Start(endpointConfig).GetAwaiter().GetResult();
            Container.Register(Component.For<IMessageSession>().Instance(endpointInstance));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);                       

            return WindsorRegistrationHelper.CreateServiceProvider(Container, services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
