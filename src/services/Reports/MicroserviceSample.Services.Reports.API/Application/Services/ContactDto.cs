namespace MicroserviceSample.Services.Reports.API.Application.Services
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
