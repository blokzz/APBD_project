using Microsoft.AspNetCore.Authorization;

namespace APBD_PROJEKT.Controllers;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Exceptions;
using Services;
[ApiController]
[Route("api/subscriptions")]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _service;

    public SubscriptionsController(ISubscriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSubscriptionDto dto)
    {
        await _service.CreateSubscription(dto);
        return Ok();
    }

    [HttpPost("{id}/pay")]
    public async Task<IActionResult> PayRenewal(int id, [FromBody] decimal amount)
    {
        await _service.PayRenewal(id, amount);
        return Ok();
    }
}