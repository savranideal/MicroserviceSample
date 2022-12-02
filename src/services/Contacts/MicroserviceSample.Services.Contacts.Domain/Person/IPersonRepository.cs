using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceSample.Services.Contacts.Domain.Person
{
    public interface IPersonRepository
    {
        Task AddAsync(PersonEntity meeting);
    }
}
