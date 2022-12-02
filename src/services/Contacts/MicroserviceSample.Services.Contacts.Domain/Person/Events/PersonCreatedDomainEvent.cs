using MicroserviceSample.BuildingBlocks.Domain;

namespace MicroserviceSample.Services.Contacts.Domain.Person.Events
{
    public class PersonCreatedDomainEvent : DomainEventBase
    {
        public Guid PersonId { get; }

        public PersonCreatedDomainEvent(Guid personId)
        {
            PersonId = personId;
        }
    }
}
