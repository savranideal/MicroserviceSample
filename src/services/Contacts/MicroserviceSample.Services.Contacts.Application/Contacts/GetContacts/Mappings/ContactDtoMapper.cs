using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts.Mappings
{
    internal static class ContactDtoMapper
    {
        public static ContactDto[] MapToDto(this IEnumerable<ContactEntity> source)
        {
            return source.Select(x => x.MapToDto()).ToArray();
        }

        public static ContactDto MapToDto(this ContactEntity cource)
        {
            return new ContactDto
            {
                Id = cource.Id,
                FirstName = cource.FirstName,
                LastName = cource.LastName,
                CompanyName = cource.CompanyName,
                Communications = cource.Communications?.Select(c => new ContactCommunicationDto
                {
                    Id = c.Id,
                    Type = Constants.CommunicationTypes.ContainsKey(c.Type) ? Constants.CommunicationTypes[c.Type] : CommunicationType.Other.ToString(),
                    Value = c.Value
                }).ToList()
            };
        }
    }
}
