namespace SagaECommerce.Contracts.Inventory.Commands;

public record CommitInventoryCommand(Guid OrderId);