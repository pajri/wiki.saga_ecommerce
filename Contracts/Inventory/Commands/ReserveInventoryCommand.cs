namespace SagaECommerce.Contracts.Inventory.Commands;

public record ReserveInventoryCommand(Guid OrderId);