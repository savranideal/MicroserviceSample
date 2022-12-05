namespace MicroserviceSample.Services.Reports.API.Application.Services
{
    public interface IContactService
    {
        Task<IEnumerable<ContactDto>> AllAsync(CancellationToken cancellationToken);
    }
}
