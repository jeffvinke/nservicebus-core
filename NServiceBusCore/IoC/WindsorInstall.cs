using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.AspNetCore.Mvc;

namespace NServiceBusCore.IoC
{
    public class WindsorInstall : IWindsorInstaller
    {      
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssembly(System.Reflection.Assembly.GetExecutingAssembly()).BasedOn<Controller>().LifestyleTransient());
        }
    }
}
