using MicroserviceSample.BuildingBlocks.Domain;

namespace MicroserviceSample.Services.Contacts.Domain.Contact.Events
{
    public class ContactCreatedDomainEvent : DomainEventBase
    {
        public Guid ContactId { get; }

        public ContactCreatedDomainEvent(Guid contactId)
        {
            ContactId = contactId;
        }
    }}
