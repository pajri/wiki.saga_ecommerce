using MassTransit;
using SagaECommerce.Contracts.Inventory.Commands;
using SagaECommerce.Contracts.Inventory.Events;

namespace SagaECommerce.InventoryService.Consumer
{
    public class CommitInventoryConsumer : IConsumer<CommitInventoryCommand>
    {
        public async Task Consume(ConsumeContext<CommitInventoryCommand> context)
        {
            Console.WriteLine($"[InventoryConsumer] Commiting inventory for OrderId: {context.Message.OrderId}");
            Console.WriteLine($"[InventoryConsumer] Inventory commited");
            await context.Publish(new InventoryCommited(context.Message.OrderId));
        }
    }
}