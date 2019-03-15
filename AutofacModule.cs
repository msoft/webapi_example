using System;
using System.Linq;
using Autofac;
using WebApiExample.Services;
using WebApiExample.Interceptors;
using Autofac.Extras.DynamicProxy;

namespace WebApiExample
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Typed registration
            builder.Register(c => new LoggingInterceptor());
            builder.Register(c => new TimingInterceptor());

            builder.RegisterType<PizzaFlavourGeneratorService>()
            .As<IPizzaFlavourGeneratorService>()
            .SingleInstance();
            builder.Register(c => new PizzaFlavourRepositoryService(c.Resolve<IServiceProvider>()))
            .As<IPizzaFlavourRepositoryService>()
            .SingleInstance();
            builder.Register(c => new PizzaOrderRepositoryService(c.Resolve<IPizzaFlavourRepositoryService>()))
            .As<IPizzaOrderRepositoryService>()
            .SingleInstance()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(LoggingInterceptor), typeof(TimingInterceptor));

            //this.RegisterObjectsByConventionBased(builder);
        }

        private void RegisterObjectsByConventionBased(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.StartsWith("WebApi")).ToArray();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsClass && t.FullName.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}