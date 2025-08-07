using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathbitChallenge.Application.DTOs;
using PathbitChallenge.Application.Interfaces;

namespace PathbitChallenge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "CLIENTE")] 
public class OrdersController : ControllerBase
{
  private readonly IOrderService _orderService;

  public OrdersController(IOrderService orderService)
  {
    _orderService = orderService;
  }

  [HttpPost]
  public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
  {
    var customerIdClaim = User.FindFirstValue("customerId");
    if (!Guid.TryParse(customerIdClaim, out var customerId))
    {
      return Forbid("A informação do cliente não foi encontrada no token.");
    }

    var result = await _orderService.CreateOrderAsync(request, customerId);

    if (result.IsFailure)
    {
      return BadRequest(new { message = result.Error });
    }

    return Ok(result.Value);
  }
}
