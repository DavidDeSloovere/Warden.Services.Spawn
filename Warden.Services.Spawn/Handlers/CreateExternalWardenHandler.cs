using System.Threading.Tasks;
using RawRabbit;
using Warden.Common.Handlers;
using Warden.Messages.Commands;
using Warden.Messages.Commands.Organizations;
using Warden.Messages.Commands.Spawn;
using Warden.Messages.Events.Organizations;

namespace Warden.Services.Spawn.Handlers
{
    public class CreateExternalWardenHandler : ICommandHandler<CreateExternalWarden>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;

        public CreateExternalWardenHandler(IHandler handler, IBusClient bus)
        {
            _handler = handler;
            _bus = bus;
        }

        public async Task HandleAsync(CreateExternalWarden command)
        {
            await _handler
                .Run(async () => 
                {
                    await Task.CompletedTask;
                })
                .OnSuccess(async () =>
                {
                    var spawnWarden = new SpawnWarden
                    {
                        Request = Request.From<SpawnWarden>(command.Request),
                        UserId = command.UserId,
                        OrganizationId = command.OrganizationId,
                        WardenId = command.WardenId,
                        Region = command.Region,
                        Name = command.Name
                    };
                    await _bus.PublishAsync(spawnWarden);
                })
                .OnCustomError(async ex => await _bus.PublishAsync(new CreateExternalWardenRejected(command.Request.Id,
                    command.UserId, ex.Code, ex.Message, command.OrganizationId)))
                .OnError(async (ex, logger) =>
                {
                    logger.Error(ex, "Error occured while creating an external warden.");
                    await _bus.PublishAsync(new CreateExternalWardenRejected(command.Request.Id,
                        command.UserId, OperationCodes.Error, ex.Message, command.OrganizationId));
                })
                .ExecuteAsync();
        }
    }
}