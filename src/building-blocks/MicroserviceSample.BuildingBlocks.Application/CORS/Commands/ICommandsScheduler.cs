using System.Threading.Tasks;

namespace MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

public interface ICommandsScheduler
{
    Task EnqueueAsync(ICommand command);
}