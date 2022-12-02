using MicroserviceSample.BuildingBlocks.Domain;

namespace MicroserviceSample.Services.Contacts.Domain.Person
{
    public class PersonContactEntity : Entity
    {
        private PersonContactEntity()
        {
            // for EF core
        }
        public Guid Id { get; private set; }
        public Guid PersonId { get; private set; }

        public PersonContactType Type { get; private set; }
        public string Value { get; set; }
    }
}
