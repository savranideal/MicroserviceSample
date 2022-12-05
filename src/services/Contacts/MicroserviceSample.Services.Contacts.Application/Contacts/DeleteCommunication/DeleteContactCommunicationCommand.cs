using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.DeleteCommunication;

public record DeleteContactCommunicationCommand(Guid ContactId, Guid Id) : IDeleteCommand<Guid>;
