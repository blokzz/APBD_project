using Microsoft.AspNetCore.Authorization;

namespace APBD_PROJEKT.Controllers;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Exceptions;
using Services;
[ApiController]
[Route("api/revenue")]
[Authorize]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _service;

    public RevenueController(IRevenueService service) => _service = service;

    [HttpGet("/current")]
    public async Task<IActionResult> GetCurrent(int? softwareId, string currency)
    {
        try
        {
            var revenue =  await _service.GetCurrentRevenue(softwareId, currency);
            return Ok(revenue);
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
    [HttpGet("/forecast")]
    public async Task<IActionResult> GetForecast(int? softwareId, string currency)
    {
        try
        {
            var revenue = await _service.GetForecastRevenue(softwareId, currency);
            return Ok(revenue);
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