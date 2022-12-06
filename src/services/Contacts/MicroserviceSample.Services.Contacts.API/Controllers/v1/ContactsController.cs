using System.Net.Mime;
using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;
using MicroserviceSample.BuildingBlocks.Application.CORS.Queries;
using MicroserviceSample.Services.Contacts.API.Features.Contacts.CreateContact;
using MicroserviceSample.Services.Contacts.API.Features.Contacts.CreateContactCommunication;
using MicroserviceSample.Services.Contacts.Application.Contacts.CreateContact;
using MicroserviceSample.Services.Contacts.Application.Contacts.CreateContactCommunication;
using MicroserviceSample.Services.Contacts.Application.Contacts.DeleteCommunication;
using MicroserviceSample.Services.Contacts.Application.Contacts.DeleteContact;
using MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace MicroserviceSample.Services.Contacts.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces(MediaTypeNames.Application.Json)]
    [Route(ApiRoute)]
    public class ContactsController : ControllerBase
    {
        private const string ApiRoute = "api/v{version:apiVersion}/contacts";
        private const string GetByIdRoute = "api/v1/contacts/{0}";
        private const string GetByIdCommunicationRoute = "api/v1/contacts/{0}/communications/{1}";
        private readonly ICommandProcessor _commandProcessor;
        private readonly IQueryProcessor _queryProcessor;

        public ContactsController(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            _commandProcessor = commandProcessor;
            _queryProcessor = queryProcessor;
        }

        /// <summary>
        /// Rehberdeki ki�ilerin listelenmesi
        /// </summary> 
        /// <returns></returns>
        /// <response code="200">Rehberdeki ki�ilerin listelendi.</response> 
        /// <response code="403">Client'�n eri�emedi�i kanal, claim ya da roleEntity.</response>
        [HttpGet]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status403Forbidden)]
        public async Task<IActionResult> Get()
        {
            ContactDto[] response = await _queryProcessor.SendAsync(new GetContactsQuery());

            return Ok(response);
        }

        /// <summary>
        /// Rehberdeki bir ki�iyle ilgili ileti�im bilgilerinin de yer ald��� detay bilgilerin getirilmesi
        /// </summary> 
        /// <returns></returns>
        /// <response code="200">Rehberdeki bir ki�iyle ilgili ileti�im bilgilerinin de yer ald��� detay bilgileri.</response> 
        /// <response code="403">Client'�n eri�emedi�i kanal, claim ya da roleEntity.</response>
        /// <response code="404">Ki�i bulunamad�</response>  
        [HttpGet("{id:guid}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            ContactDto response = await _queryProcessor.SendAsync(new GetContactQuery(id));

            return Ok(response);
        }

        /// <summary>
        /// Rehberde ki�i olu�turma
        /// </summary> 
        /// <returns></returns>
        /// <response code="201">Yeni ki�i olu�turuldu.</response>
        /// <response code="400">Model'de hatal� ya da i�lem ger�ekle�tirilirken hata olu�tu.</response>  
        /// <response code="403">Client'�n eri�emedi�i kanal, claim ya da roleEntity.</response>
        [HttpPost]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
        {
            Guid response = await _commandProcessor.SendAsync(new CreateContactCommand(request.FirstName, request.LastName, request.Company, request.Communications.Select(c => (c.Type, c.Value)).ToList()));

            return Created(string.Format(GetByIdRoute,response), response);
        }

        /// <summary>
        /// Rehberde ki�i kald�rma
        /// </summary> 
        /// <returns></returns>
        /// <response code="204">Ki�i silindi</response>
        /// <response code="403">Client'�n eri�emedi�i kanal, claim.</response>
        /// <response code="404">Ki�i bulunamad�</response>  
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            await _commandProcessor.SendAsync(new DeleteContactCommand(id));

            return NoContent();
        }

        /// <summary>
        /// Rehberdeki ki�iye ileti�im bilgisi ekleme
        /// </summary> 
        /// <returns></returns>
        /// <response code="201">Rehberdeki ki�iye ileti�im bilgisi eklendi</response>
        /// <response code="400">Model'de hatal� ya da i�lem ger�ekle�tirilirken hata olu�tu.</response>  
        /// <response code="403">Client'�n eri�emedi�i kanal, claim.</response>
        /// <response code="404">Rehberdeki ki�i bulunamad�</response>
        [HttpPost("{contactId:guid}/communications")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> AddCommunication([FromRoute] Guid contactId, [FromBody] CreateContactCommunicationRequest request)
        {
            Guid response = await _commandProcessor.SendAsync(new CreateContactCommunicationCommand(contactId, request.Type, request.Value));

            return Created(string.Format(GetByIdCommunicationRoute,contactId,response), response);
        }

        /// <summary>
        /// Rehberdeki ki�iden ileti�im bilgisi kald�rma
        /// </summary> 
        /// <returns></returns>
        /// <response code="204">Rehberdeki ki�iden ileti�im bilgisi kald�r�ld�.</response> 
        /// <response code="403">Client'�n eri�emedi�i kanal, claim.</response>
        /// <response code="404">Ki�i veya ki�i bilgisi bulunamad�</response>
        [HttpDelete("{contactid:guid}/communications/{id:guid}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> DeleteCommunication(Guid contactid, Guid id)
        {
            await _commandProcessor.SendAsync(new DeleteContactCommunicationCommand(contactid, id));

            return NoContent();
        }
    }
}