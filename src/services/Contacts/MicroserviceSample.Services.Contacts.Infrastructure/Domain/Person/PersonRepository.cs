using MicroserviceSample.Services.Contacts.Domain.Person;
using MicroserviceSample.Services.Contacts.Infrastructure.Persistence;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Domain.Person
{
    internal class PersonRepository : IPersonRepository
    {
        private readonly ContactManagementDbContext _dbContext;

        public PersonRepository(ContactManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(PersonEntity person)
        {
            _dbContext.Add(person);
        }

        public async Task AddAsync(PersonEntity person)
        {
            await _dbContext.AddAsync(person);
        }
    }
}
