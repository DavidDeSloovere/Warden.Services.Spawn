using Warden.Common.Host;
using Warden.Services.Spawn.Framework;
using Warden.Messages.Commands.Organizations;

namespace Warden.Services.Spawn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 5056)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<CreateExternalWarden>()
                .Build()
                .Run();
        }
    }
}
