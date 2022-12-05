using MediatR;

using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;
using MicroserviceSample.BuildingBlocks.Application.Exception.Types;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.DeleteContact
{
    internal class DeleteContactCommandHandler : ICommandHandler<DeleteContactCommand, Unit>
    {
        private readonly IContactRepository _contactRepository;

        public DeleteContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {

            ContactEntity contact = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);
            if (contact != null)
            {
                _contactRepository.Remove(contact);
            }
            else
            {
                throw new ResourceNotFoundException("contact not found");
            }
             
            return Unit.Value;
        }
    }
}
