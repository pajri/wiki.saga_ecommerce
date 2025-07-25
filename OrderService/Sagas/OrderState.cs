using MassTransit;

namespace SagaECommerce.OrderService.Sagas;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = String.Empty;
} 