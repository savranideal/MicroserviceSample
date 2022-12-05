using System.Net;

namespace MicroserviceSample.BuildingBlocks.Application.Exception.Types;

public class ConflictException : CustomException
{
    public ConflictException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.Conflict;
    }
}
