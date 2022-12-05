using MediatR;

using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.DeleteContact
{
    public record DeleteContactCommand : IDeleteCommand<Guid>
    {
        public Guid Id { get; init; }

        public DeleteContactCommand(Guid id)
        {
            Id = id;
        }
    }
}
