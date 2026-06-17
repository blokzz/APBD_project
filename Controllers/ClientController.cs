using Microsoft.AspNetCore.Authorization;

namespace APBD_PROJEKT.Controllers;
using Microsoft.AspNetCore.Mvc;
using Dtos.Clients;
using Exceptions;
using Services;
[ApiController]
[Route("api/client")]
[Authorize]
public class ClientController : ControllerBase
{
    private readonly IClientService _service;

    public ClientController(IClientService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        try
        {
            var clients = await _service.GetClients();
            return Ok(clients);
        }
        catch (ConflictException e)
        {
            return  Conflict(e.Message);
        }
        catch (NotFoundException e)
        {
            return  NotFound(e.Message);
        }
    }
    
    [HttpPost("/individual")]
    public async Task<IActionResult> CreateIndividual([FromBody] CreateIndividualClientDto client)
    {
        try
        {
            await _service.CreateIndividualClient(client);
            return Created();
        }
        catch (ConflictException e)
        {
            return  Conflict(e.Message);
        }
        catch (NotFoundException e)
        {
            return  NotFound(e.Message);
        }
    }
    
    [HttpPost("/company")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyClientDto client)
    {
        try
        {
            await _service.CreateCompanyClient(client);
            return Created();
        }
        catch (ConflictException e)
        {
            return  Conflict(e.Message);
        }
        catch (NotFoundException e)
        {
            return  NotFound(e.Message);
        }
        
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteClient(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return  NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return  BadRequest(e.Message);
        }
    }
    
    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateClient([FromRoute] int id, [FromBody] UpdateClientDto dto)
    {
        try
        {
            await _service.UpdateClient(id, dto);
            return NoContent();
        }
        catch (ConflictException e)
        {
            return  Conflict(e.Message);
        }
        catch (NotFoundException e)
        {
            return  NotFound(e.Message);
        }

    }
}