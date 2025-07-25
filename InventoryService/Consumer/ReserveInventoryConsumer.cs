using MassTransit;
using SagaECommerce.Contracts.Inventory.Commands;
using SagaECommerce.Contracts.Inventory.Events;

namespace SagaECommerce.InventoryService.Consumer
{
    public class ReserveInventoryConsumer : IConsumer<ReserveInventoryCommand>
    {
        public async Task Consume(ConsumeContext<ReserveInventoryCommand> context)
        {
            Console.WriteLine($"[InventoryConsumer] Reserving inventory for OrderId: {context.Message.OrderId}");
            Console.WriteLine($"[InventoryConsumer] Inventory reserved");
            await context.Publish(new InventoryReserved(context.Message.OrderId));
        }
    }
}