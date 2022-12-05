using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;
using MicroserviceSample.Services.Contacts.Domain.Contact;

using Microsoft.EntityFrameworkCore;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.CreateContact
{
    internal class CreateContactCommandHandler : ICommandHandler<CreateContactCommand, Guid>
    {
        private readonly IContactRepository _contactRepository;

        public CreateContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public Task<Guid> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {

            ContactEntity contactEntity = ContactEntity.Create(request.FirstName, request.LastName, request.Company);

            if (request.Communications != null)
            {
                foreach ((string type, string value) communication in request.Communications)
                {
                    CommunicationType type = CommunicationType.Other;
                    if (Constants.CommunicationTypes.Any(c => c.Value == communication.type.ToLower()))
                    {
                        type = Constants.CommunicationTypes.First(c => c.Value == communication.type.ToLower()).Key;
                    }

                    contactEntity.AddCommunication(type, communication.value);
                }
            }

            _contactRepository.Add(contactEntity); 

            return Task.FromResult(contactEntity.Id);
        }
    }
}
