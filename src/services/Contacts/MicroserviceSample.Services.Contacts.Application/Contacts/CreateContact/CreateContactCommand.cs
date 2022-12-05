using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.CreateContact
{
    public record CreateContactCommand : ICreateCommand<Guid>
    {
        public CreateContactCommand(string firstName, string lastName, string company, List<(string type, string value)>? communications)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Communications = communications;
        }

        internal string FirstName { get; }
        internal string LastName { get; }
        internal string Company { get; }
        internal List<(string type, string value)>? Communications { get; }
    }
}
