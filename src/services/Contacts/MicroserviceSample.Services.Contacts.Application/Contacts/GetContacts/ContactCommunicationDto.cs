namespace MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts
{
    public class ContactCommunicationDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
