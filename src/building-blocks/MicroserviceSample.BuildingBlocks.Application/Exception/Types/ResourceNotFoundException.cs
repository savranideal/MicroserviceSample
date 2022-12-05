using System.Net;

namespace MicroserviceSample.BuildingBlocks.Application.Exception.Types;

public class ResourceNotFoundException : CustomException
{
    public ResourceNotFoundException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.NotFound;
    }
}
