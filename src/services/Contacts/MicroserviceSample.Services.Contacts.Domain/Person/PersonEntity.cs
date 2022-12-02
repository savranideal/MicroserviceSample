using System.Xml.Linq;

using MicroserviceSample.BuildingBlocks.Domain;
using MicroserviceSample.Services.Contacts.Domain.Person.Events;

namespace MicroserviceSample.Services.Contacts.Domain.Person
{
    public class PersonEntity : Entity
    {
        private PersonEntity()
        {
            /// for ef core
        }

        private PersonEntity(Guid id, string firstname, string lastname, string companyName)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            CompanyName = companyName;
            AddDomainEvent(new PersonCreatedDomainEvent(Id));
        }

        public Guid Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string CompanyName { get; private set; }
         
        public virtual ICollection<PersonContactEntity> Contacts { get; private set; }

        public static PersonEntity Create(Guid id, string firstname, string lastname, string companyName)
        {
            return new PersonEntity(id, firstname, lastname, companyName);
        }

    }
}
