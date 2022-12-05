using FluentValidation;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.CreateContact
{
    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty()
                .WithMessage("Firstname cannot be empty.");
        }
    }
}