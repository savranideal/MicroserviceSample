using MicroserviceSample.Services.Contacts.Domain.Contact;
using MicroserviceSample.Services.Contacts.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Domain.Contact
{
    internal class ContactRepository : IContactRepository
    {
        private readonly ContactManagementDbContext _dbContext;

        public ContactRepository(ContactManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(ContactEntity Contact)
        {
            _dbContext.Add(Contact);
        }

        public async Task<IEnumerable<ContactEntity>> All(CancellationToken cancellationToken)
        {
            return await _dbContext.Contacts.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<ContactEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public void Remove(ContactEntity contact)
        {
            _dbContext.Remove(contact);
        }
    }
}
