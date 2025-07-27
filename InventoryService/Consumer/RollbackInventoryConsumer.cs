using MassTransit;
using SagaECommerce.Contracts.Inventory.Commands;
using SagaECommerce.Contracts.Inventory.Events;

namespace SagaECommerce.InventoryService.Consumer
{
    public class RollbackInventoryConsumer : IConsumer<RollbackInventoryCommand>
    {
        public async Task Consume(ConsumeContext<RollbackInventoryCommand> context)
        {
            Console.WriteLine($"[InventoryConsumer] Rolling back inventory for OrderId: {context.Message.OrderId}");
            Console.WriteLine($"[InventoryConsumer] Inventory restored");
            await context.Publish(new InventoryRestored(context.Message.OrderId));
        }
    }
}