namespace MicroserviceSample.Services.Reports.API.Application.Services
{
    public class HttpClientCorrelationIdDelegatingHandler : DelegatingHandler
    {
        private const string CorrelationIdHeaderName = "X-Correlation-ID";
        public HttpClientCorrelationIdDelegatingHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put || request.Method == HttpMethod.Delete)
            {
                if (!request.Headers.Contains(CorrelationIdHeaderName))
                {
                    request.Headers.Add(CorrelationIdHeaderName, Guid.NewGuid().ToString());
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
