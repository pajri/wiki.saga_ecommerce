namespace SagaECommerce.Contracts.Payment.Commands;

public record ProcessPaymentCommand(Guid OrderId);