using MicroserviceSample.BuildingBlocks.Application.CORS.Queries;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts
{
    public record GetContactQuery(Guid Id) : IQuery<ContactDto>;
}
