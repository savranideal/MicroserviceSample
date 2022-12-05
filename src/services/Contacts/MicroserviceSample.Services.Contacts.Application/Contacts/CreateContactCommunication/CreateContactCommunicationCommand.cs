using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.CreateContactCommunication
{
    public record CreateContactCommunicationCommand : ICreateCommand<Guid>
    {
        public CreateContactCommunicationCommand(Guid contactId, string type, string value)
        {
            ContactId = contactId;
            Type = type;
            Value = value;
        }

        public Guid ContactId { get; }
        public string Type { get; }
        public string Value { get; }
    }
}
