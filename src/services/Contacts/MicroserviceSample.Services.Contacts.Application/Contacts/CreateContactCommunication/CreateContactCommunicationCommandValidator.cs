using FluentValidation;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.CreateContactCommunication
{
    public class CreateContactCommunicationCommandValidator : AbstractValidator<CreateContactCommunicationCommand>
    {
        public CreateContactCommunicationCommandValidator()
        {
            RuleFor(c => c.ContactId).NotEmpty()
                .WithMessage("Contact is required");

            RuleFor(c => c.Type).NotEmpty()
                .WithMessage("Type cannot be empty.");
        }
    }
}