using System.Text.Json.Serialization;

namespace MicroserviceSample.Services.Reports.API.Application.Services
{
    public class ContactDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonPropertyName("communications")]
        public List<ContactCommunicationDto>? Communications { get; set; }
    }
}
