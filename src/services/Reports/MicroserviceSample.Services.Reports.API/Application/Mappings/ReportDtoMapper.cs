using MicroserviceSample.Services.Reports.API.Domain;

namespace MicroserviceSample.Services.Reports.API.Application.Mappings
{
    internal static class ReportDtoMapper
    {
        public static ReportDto[] MapToDto(this IEnumerable<ReportEntity> source, string host)
        {
            return source.Select(x => x.MapToDto(host)).ToArray();
        }

        public static ReportDto MapToDto(this ReportEntity source, string host)
        {
            return new ReportDto
            {
                Id = source.Id,
                RequestDate = source.RequestDate,
                Path = string.IsNullOrEmpty(source.Path) ? string.Empty : $"{host}{source.Path}",
                Status = source.Status.ToString()
            };
        }
    }
}
