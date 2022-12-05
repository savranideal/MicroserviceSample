using MicroserviceSample.BuildingBlocks.Application.CORS.Queries;
using MicroserviceSample.BuildingBlocks.Application.Exception.Types;
using MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts.Mappings;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts
{
    internal class GetContactQueryQueryHandler : IQueryHandler<GetContactsQuery, ContactDto[]>, IQueryHandler<GetContactQuery, ContactDto>
    {
        private readonly IContactRepository _contactRepository;

        public GetContactQueryQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<ContactDto[]> Handle(GetContactsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<ContactEntity> contacts = await _contactRepository.All(cancellationToken);

            return contacts.MapToDto();
        }

        public async Task<ContactDto> Handle(GetContactQuery request, CancellationToken cancellationToken)
        {
            ContactEntity contact = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);

            if (contact == null)
            {
                throw new ResourceNotFoundException("contact not found");
            }

            return contact.MapToDto();
        }
    }
}
