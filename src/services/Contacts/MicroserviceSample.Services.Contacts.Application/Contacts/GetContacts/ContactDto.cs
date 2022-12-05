namespace MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }

        public List<ContactCommunicationDto>? Communications { get; set; }
    }
}
