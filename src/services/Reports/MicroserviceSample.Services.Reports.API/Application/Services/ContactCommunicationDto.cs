using System.Text.Json.Serialization;

namespace MicroserviceSample.Services.Reports.API.Application.Services
{
    public class ContactCommunicationDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
