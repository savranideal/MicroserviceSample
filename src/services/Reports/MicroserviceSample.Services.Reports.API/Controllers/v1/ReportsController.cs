using System.Net.Mime;
using MicroserviceSample.BuildingBlocks.Infrastructure.Persistence;
using MicroserviceSample.Services.Reports.API.Application.Mappings;
using MicroserviceSample.Services.Reports.API.Domain;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes; 

namespace MicroserviceSample.Services.Reports.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Produces(MediaTypeNames.Application.Json)]
[Route(ApiRoute)]
public class ReportsController : ControllerBase
{
    private const string ApiRoute = "api/v{version:apiVersion}/reports";
    private readonly IReportRepository _reportRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public ReportsController(IReportRepository reportRepository, IUnitOfWork unitOfWork,IConfiguration configuration)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    /// <summary>
    /// Sistemin olu�turdu�u raporlar�n listelenmesi
    /// </summary> 
    /// <returns></returns>
    /// <response code="200">Sistemin olu�turdu�u raporlar�n listesi.</response> 
    /// <response code="403">Client'�n eri�emedi�i kanal, claim.</response>
    [HttpGet]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status403Forbidden)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        IEnumerable<ReportEntity> response = await _reportRepository.AllAsync(cancellationToken);
        string host=_configuration.GetValue<string>("Services:Report")!;
        return Ok(response.MapToDto(host));
    }

    /// <summary>
    /// Sistemin olu�turdu�u bir raporun detay bilgilerinin getirilmesi
    /// </summary> 
    /// <returns></returns>
    /// <response code="200">Sistemin olu�turdu�u bir raporun detay bilgileri.</response> 
    /// <response code="403">Client'�n eri�emedi�i kanal, claim.</response>
    /// <response code="404">Rapor bulunamad�</response>  
    [HttpGet("{id:guid}")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status403Forbidden)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        ReportEntity response = await _reportRepository.GetById(id, cancellationToken);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (response == null)
        {
            return NotFound();
        }
        
        string host=_configuration.GetValue<string>("Services:Report")!;
        return Ok(response.MapToDto(host));
    }

    /// <summary>
    /// Rehberdeki ki�ilerin bulunduklar� konuma g�re istatistiklerini ��kartan bir rapor talebi
    /// </summary> 
    /// <returns></returns>
    /// <response code="201">Rehberdeki ki�ilerin bulunduklar� konuma g�re istatistiklerini ��kartan bir rapor talebi al�nd�.</response>
    /// <response code="400">Devam eden i�lem var.</response>  
    /// <response code="403">Client'�n eri�emedi�i kanal, claim.</response>
    [HttpPost]
    [ProducesResponseType(Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status403Forbidden)]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        bool isInProgress = await _reportRepository.HasActiveReportAsync(cancellationToken);

        if (isInProgress)
        {
            return BadRequest("there is a exists request.");
        }

        ReportEntity entity = ReportEntity.Create();

        _reportRepository.Add(entity);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Accepted("", entity.Id);
    }
}