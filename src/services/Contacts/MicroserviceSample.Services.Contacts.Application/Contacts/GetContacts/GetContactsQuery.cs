using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroserviceSample.BuildingBlocks.Application.CORS.Queries;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts
{
    public record GetContactsQuery : IQuery<ContactDto[]>
    {
    }
}
