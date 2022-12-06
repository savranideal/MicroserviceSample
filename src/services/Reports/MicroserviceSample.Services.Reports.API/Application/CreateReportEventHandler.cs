using System.Text;
using DotNetCore.CAP;
using MicroserviceSample.BuildingBlocks.Infrastructure.Persistence;using MicroserviceSample.Services.Reports.API.Application.Events;
using MicroserviceSample.Services.Reports.API.Application.Services;
using MicroserviceSample.Services.Reports.API.Domain;

namespace MicroserviceSample.Services.Reports.API.Application
{
    public class CreateReportEventHandler : ICapSubscribe
    {
        private readonly IContactService _contactService;
        private readonly IReportRepository _reportRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IUnitOfWork _unitOfWork;
        private const string UndefinedLocation = "undefined";
        private const string LocationKey = "location";
        private const string PhoneKey = "phone";

        public CreateReportEventHandler(IContactService contactService, IReportRepository reportRepository,
            IWebHostEnvironment environment, IUnitOfWork unitOfWork)
        {
            _contactService = contactService;
            _reportRepository = reportRepository;
            _environment = environment;
            _unitOfWork = unitOfWork;
        }

        [CapSubscribe(nameof(ReportCreatedIntegrationEvent))]
        public async Task Handle(ReportCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            IEnumerable<ContactDto> response = await _contactService.AllAsync(cancellationToken);
           
            Dictionary<string, ReportData> reportData = new();
            foreach (ContactDto contact in response)
            {
                if (contact.Communications == null)
                {
                    if (!reportData.ContainsKey(UndefinedLocation))
                    {
                        reportData[UndefinedLocation] = new ReportData() { ContactCount = 1 };
                    }
                    else
                    {
                        reportData[UndefinedLocation].ContactCount += 1;
                    }

                    continue;
                }

                int phoneCount = contact.Communications.Count(c => c.Type == PhoneKey);
                if (contact.Communications != null && contact.Communications.Any(c => c.Type == LocationKey))
                {
                    ContactCommunicationDto[] locations = contact.Communications.Where(c => c.Type == LocationKey).ToArray();
                    foreach (ContactCommunicationDto location in locations)
                    {
                        if (!reportData.ContainsKey(location.Value))
                        {
                            reportData[location.Value] = new ReportData { ContactCount = 1, PhoneCount = phoneCount };
                        }
                        else
                        {
                            reportData[location.Value].ContactCount += 1;
                            reportData[location.Value].PhoneCount += phoneCount;
                        }
                    }
                }
            }
              
            ReportEntity entity = await _reportRepository.GetById(@event.ReportId, cancellationToken);
            string fileName = $"{entity.RequestDate.ToString("yyyy-MM-dd")}_{@event.ReportId}.csv"; 
            string reportFilePath = Path.Combine(_environment.WebRootPath, "Reports", fileName);
            StringBuilder reportCsvBuilder = new();
            reportCsvBuilder.AppendFormat("{0},{1},{2}{3}", "Location", "Contact", "Phone", Environment.NewLine);

            foreach (KeyValuePair<string, ReportData> item in reportData)
            {
                reportCsvBuilder.AppendFormat("{0},{1},{2}{3}", item.Key, item.Value.ContactCount,
                    item.Value.PhoneCount, Environment.NewLine);
            }

            File.AppendAllText(reportFilePath, reportCsvBuilder.ToString());

            entity.Status = ReportStatus.Completed;
            entity.Path = reportFilePath;
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private class ReportData
        {
            public int ContactCount { get; set; }
            public int PhoneCount { get; set; }
        }
    }
}
