using System.Net;

namespace MicroserviceSample.BuildingBlocks.Application.Exception.Types;

public class BadRequestException : CustomException
{
    public BadRequestException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.NotFound;
    }
}
