using Microsoft.AspNetCore.Authorization;

namespace APBD_PROJEKT.Controllers;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Exceptions;
using Services;
[ApiController]
[Route("api/contract")]
[Authorize]
public class ContractController : ControllerBase
{
    private readonly IContractService _service;

    public ContractController(IContractService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create(CreateContractDto contract)
    {
        try
        {
            await _service.CreateContract(contract);
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
    [HttpPost("/payment")]
    public async Task<IActionResult> CreatePayment(int id , decimal amount)
    {
        try
        {
            await _service.AddPayment(id, amount);
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
}