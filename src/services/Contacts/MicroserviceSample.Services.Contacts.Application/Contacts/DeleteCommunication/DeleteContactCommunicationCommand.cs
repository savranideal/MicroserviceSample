using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.DeleteContactCommunication;

public record DeleteContactCommunicationCommand : IDeleteCommand<Guid>
{
    public DeleteContactCommunicationCommand(Guid contactId, Guid id)
    {
        ContactId = contactId;
        Id = id;
    }

    public Guid ContactId { get; }
    public Guid Id { get; init; }
}
