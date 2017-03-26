using System;
using System.Net;
using System.Threading.Tasks;
using Warden.Core;
using Warden.Messages.Commands;
using Warden.Messages.Commands.Spawn;
using Warden.Services.Spawn.Services;
using Warden.Watchers.Web;

namespace Warden.Services.Spawn.Handlers
{
    public class SpawnWardenHandler : ICommandHandler<SpawnWarden>
    {
        //private readonly IWardenService _wardenService;
        //private readonly IWardenConfigurationService _wardenConfigurationService;
        //private readonly ISecuredRequestService _securedRequestService;

        //public SpawnWardenHandler(IWardenService wardenService, 
        //    IWardenConfigurationService wardenConfigurationService,
        //    ISecuredRequestService securedRequestService)
        //{
        //    _wardenService = wardenService;
        //    _wardenConfigurationService = wardenConfigurationService;
        //    _securedRequestService = securedRequestService;
        //}

        //public async Task HandleAsync(SpawnWarden command)
        //{
        //    var securedRequestId = Guid.NewGuid();
        //    var configurationId = Guid.NewGuid();
        //    await _wardenConfigurationService.CreateAsync(configurationId, command.Configuration);
        //    await _securedRequestService.CreateAsync(securedRequestId, ResourceType.WardenConfiguration, configurationId);
        //    var securedRequest = await _securedRequestService.GetAsync(securedRequestId);
        //    //await _bus.Publish(new Common.Commands.SpawnWarden(command.AuthenticatedUserId,
        //    //    configurationId.ToString(), securedRequest.Value.Token, command.Region));
        //}

        private readonly IWardenHostService _wardenHostService;

        public SpawnWardenHandler(IWardenHostService wardenHostService)
        {
            _wardenHostService = wardenHostService;
        }

        public async Task HandleAsync(SpawnWarden command)
        {
            var name = $"Warden {Guid.NewGuid()}";
            await _wardenHostService.AddWardenAsync(command.UserId, CreateWarden(name));
            await _wardenHostService.StartWardenAsync(command.UserId, name);
        }

        private static IWarden CreateWarden(string name, int interval = 5)
        {
            var wardenConfiguration = WardenConfiguration
                .Create()
                .AddWebWatcher("http://httpstat.us/200", cfg =>
                {
                    cfg.EnsureThat(response => response.StatusCode == HttpStatusCode.Accepted);
                })  
                .SetGlobalWatcherHooks((hooks, integrations) =>
                {
                    Console.WriteLine($"Hello!");
                })
                .WithInterval(TimeSpan.FromSeconds(interval))
                .WithConsoleLogger()
                .Build();

            return WardenInstance.Create(name,wardenConfiguration);
        }
    }
}