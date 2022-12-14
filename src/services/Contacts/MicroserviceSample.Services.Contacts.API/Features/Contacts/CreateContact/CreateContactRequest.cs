namespace MicroserviceSample.Services.Contacts.API.Features.Contacts.CreateContact;

public class CreateContactRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }

    public List<ContractCommunication>  Communications{ get; set; }
}
