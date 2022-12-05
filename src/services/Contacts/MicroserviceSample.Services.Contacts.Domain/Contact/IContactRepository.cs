using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceSample.Services.Contacts.Domain.Contact
{
    public interface IContactRepository
    {

        void Add(ContactEntity contact);
        Task<IEnumerable<ContactEntity>> All(CancellationToken cancellationToken);
        Task<ContactEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        void Remove(ContactEntity contact);
    }
}
