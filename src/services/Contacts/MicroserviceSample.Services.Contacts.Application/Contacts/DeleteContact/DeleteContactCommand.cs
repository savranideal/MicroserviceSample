using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.DeleteContact
{
    public record DeleteContactCommand(Guid Id) : IDeleteCommand<Guid>;
}
