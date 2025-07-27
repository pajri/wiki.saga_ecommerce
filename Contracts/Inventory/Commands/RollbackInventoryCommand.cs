namespace SagaECommerce.Contracts.Inventory.Commands;

public record RollbackInventoryCommand(Guid OrderId);