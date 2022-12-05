using MicroserviceSample.BuildingBlocks.Application.CORS.Queries;

namespace MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts
{
    public record GetContactQuery : IQuery<ContactDto>
    { 
        public GetContactQuery(Guid id)
        {
            Id = id;
        } 

        public Guid Id { get; init; }
    }
}
