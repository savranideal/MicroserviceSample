using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;
using MicroserviceSample.BuildingBlocks.Application.Exception.Types;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.CreateContactCommunication
{
    internal class CreateContactCommunicationCommandHandler : ICommandHandler<CreateContactCommunicationCommand, Guid>
    {
        private readonly IContactRepository _contactRepository;

        public CreateContactCommunicationCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<Guid> Handle(CreateContactCommunicationCommand request, CancellationToken cancellationToken)
        {
            ContactEntity contact = await _contactRepository.GetByIdAsync(request.ContactId, cancellationToken);
            if (contact == null)
            {
                throw new ResourceNotFoundException("Contact not found");
            }

            CommunicationType type = CommunicationType.Other;

            if (Constants.CommunicationTypes.Any(c => c.Value == request.Type.ToLower()))
            {
                type = Constants.CommunicationTypes.First(c => c.Value == request.Type.ToLower()).Key;
            }

            if (contact.Communications.Any(c => c.Type == type && c.Value == request.Value))
            {
                throw new ConflictException("Communication is conflict");
            }

            Guid communicationId = contact.AddCommunication(type, request.Value);

            return communicationId;
        }
    }
}
