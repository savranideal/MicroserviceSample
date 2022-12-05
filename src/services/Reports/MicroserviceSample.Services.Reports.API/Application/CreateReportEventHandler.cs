using DotNetCore.CAP;

using MicroserviceSample.Services.Reports.API.Application.Events;
using MicroserviceSample.Services.Reports.API.Application.Services;

namespace MicroserviceSample.Services.Reports.API.Application
{
    public class CreateReportEventHandler
    {
        private readonly IContactService _contactService;

        public CreateReportEventHandler(IContactService contactService)
        {
            _contactService = contactService;
        }

        [CapSubscribe(nameof(ReportCreatedIntegrationEvent))]
        public async Task Handle(ReportCreatedIntegrationEvent request, CancellationToken cancellationToken)
        {
           IEnumerable<ContactDto> response= await _contactService.AllAsync(cancellationToken);
        }
    }
}
