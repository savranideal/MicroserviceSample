using MicroserviceSample.Services.Reports.API.Domain;

namespace MicroserviceSample.Services.Reports.API.Application.Mappings
{
    internal static class ReportDtoMapper
    {

        public static ReportDto[] MapToDto(this IEnumerable<ReportEntity> source)
        {
            return source.Select(x => x.MapToDto()).ToArray();
        }

        public static ReportDto MapToDto(this ReportEntity source)
        {
            return new ReportDto
            {
                Id = source.Id,
                RequestDate = source.RequestDate,
                Path = source.Path,
                Status = source.Status.ToString()
            };
        }
    }
}
