using System.Net.Mime;
using MicroserviceSample.BuildingBlocks.Infrastructure.Persistence;
using MicroserviceSample.Services.Reports.API.Application;
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

    public ReportsController(IReportRepository reportRepository, IUnitOfWork unitOfWork)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Sistemin oluþturduðu raporlarýn listelenmesi
    /// </summary> 
    /// <returns></returns>
    /// <response code="200">Sistemin oluþturduðu raporlarýn listesi.</response> 
    /// <response code="403">Client'ýn eriþemediði kanal, claim ya da roleEntity.</response>
    [HttpGet]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status403Forbidden)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        IEnumerable<ReportEntity> response = await _reportRepository.AllAsync(cancellationToken);

        return Ok(response.MapToDto());
    }

    /// <summary>
    /// Sistemin oluþturduðu bir raporun detay bilgilerinin getirilmesi
    /// </summary> 
    /// <returns></returns>
    /// <response code="200">Sistemin oluþturduðu bir raporun detay bilgileri.</response> 
    /// <response code="403">Client'ýn eriþemediði kanal, claim ya da roleEntity.</response>
    /// <response code="404">Rapor bulunamadý</response>  
    [HttpGet("{id:guid}")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status403Forbidden)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        ReportEntity response = await _reportRepository.GetById(id, cancellationToken);
        if (response == null)
        {
            return NotFound();
        }

        return Ok(response.MapToDto());
    }

    /// <summary>
    /// Rehberdeki kiþilerin bulunduklarý konuma göre istatistiklerini çýkartan bir rapor talebi
    /// </summary> 
    /// <returns></returns>
    /// <response code="201">Yeni kiþi oluþturuldu.</response>
    /// <response code="400">Devam eden iþlem var.</response>  
    /// <response code="403">Client'ýn eriþemediði kanal, claim ya da roleEntity.</response>
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

        ReportEntity entity = new()
        {
            RequestDate = DateTimeExtensions.LocalNow,
            Status = ReportStatus.InProgress
        };

        _reportRepository.Add(entity);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Accepted("", entity.Id);
    }
}