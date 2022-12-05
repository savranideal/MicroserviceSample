namespace MicroserviceSample.Services.Reports.API.Application.Services
{
    public interface IContactService
    {
        Task<IEnumerable<ContactDto>> All(CancellationToken cancellationToken);
    }
}
