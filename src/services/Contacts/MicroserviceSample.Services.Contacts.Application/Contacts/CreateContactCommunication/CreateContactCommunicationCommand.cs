using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.CreateContactCommunication
{
    public record CreateContactCommunicationCommand(Guid ContactId, string Type, string Value) : ICreateCommand<Guid>;
}
