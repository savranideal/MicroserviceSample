using MediatR;

using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;
using MicroserviceSample.BuildingBlocks.Application.Exception.Types;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.DeleteContactCommunication
{
    internal class DeleteContactCommunicationCommandHandler : ICommandHandler<DeleteContactCommunicationCommand, Unit>
    {
        private readonly IContactRepository _contactRepository;

        public DeleteContactCommunicationCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<Unit> Handle(DeleteContactCommunicationCommand request, CancellationToken cancellationToken)
        {

            ContactEntity contact = await _contactRepository.GetByIdAsync(request.ContactId, cancellationToken);
            if (contact == null)
            {
                throw new ResourceNotFoundException("Contact not found");
            }

            ContactCommunicationEntity? communicationEntity = contact.Communications.FirstOrDefault(c => c.Id == request.Id);

            if (communicationEntity == null)
            {
                throw new ResourceNotFoundException("Communication not found");
            }

            contact.DeleteCommunication(communicationEntity);

            return Unit.Value;
        }
    }
}
