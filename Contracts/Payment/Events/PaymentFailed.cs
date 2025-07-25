namespace SagaECommerce.Contracts.Payment.Events;

public record PaymentFailed(Guid OrderId);