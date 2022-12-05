using System.Text.Json;

namespace MicroserviceSample.Services.Reports.API.Application.Services
{
    public class ContactService : IContactService
    {
        private const string ContactApiUrl = "api/v1/contacts";
        private readonly HttpClient _httpClient;

        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ContactDto>> All(CancellationToken cancellationToken)
        {
            using HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(ContactApiUrl, cancellationToken);

            string httpContent = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<IEnumerable<ContactDto>>(httpContent);
        }
    }
}