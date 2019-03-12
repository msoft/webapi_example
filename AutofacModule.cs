using System;
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
            builder.Register(c => new CallLogger(Console.Out));

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
            .InterceptedBy(typeof(CallLogger));;
        }
    }
}