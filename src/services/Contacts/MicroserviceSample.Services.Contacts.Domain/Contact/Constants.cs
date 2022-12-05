namespace MicroserviceSample.Services.Contacts.Domain.Contact
{
    public static class Constants
    {
        public static readonly IReadOnlyDictionary<CommunicationType, string> CommunicationTypes = new Dictionary<CommunicationType, string>
        {
            { CommunicationType.Phone,"phone"},
            { CommunicationType.Location,"location"},
            { CommunicationType.Email,"email"},
        }.AsReadOnly();
    }
}
