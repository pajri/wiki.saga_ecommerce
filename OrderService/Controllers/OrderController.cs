using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SagaECommerce.Contracts.Order;
using SagaECommerce.Contracts.Order.Events;

namespace SagaECommerce.OrderService.Controllers;

public record SubmitOrderRequest(Guid orderId);

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderRequest request)
    {
        await _publishEndpoint.Publish(new OrderCreated(request.orderId));
        return Ok(new { Message = "Order Created", OrderId = request.orderId });
    }
}