using MicroserviceSample.BuildingBlocks.Application.CORS.Queries;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts
{
    public record GetContactsQuery : IQuery<ContactDto[]>
    {
    }
}
