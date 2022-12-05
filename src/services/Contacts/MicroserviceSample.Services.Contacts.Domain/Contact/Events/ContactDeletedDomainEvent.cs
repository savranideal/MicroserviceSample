using MicroserviceSample.BuildingBlocks.Domain;

namespace MicroserviceSample.Services.Contacts.Domain.Contact.Events
{
    public class ContactDeletedDomainEvent : DomainEventBase
    {
        public Guid ContactId { get; }

        public ContactDeletedDomainEvent(Guid contactId)
        {
            ContactId = contactId;
        }
    }
}
