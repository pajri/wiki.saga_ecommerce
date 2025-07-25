using MassTransit;
using SagaECommerce.Contracts.Payment.Commands;
using SagaECommerce.Contracts.Payment.Events;

namespace SagaECommerce.PaymentService.Consumer
{
    public class PaymentConsumer: IConsumer<ProcessPaymentCommand>
    {
        public async Task Consume(ConsumeContext<ProcessPaymentCommand> context)
        {
            Console.WriteLine($"[PaymentConsumer] Processing payment for OrderId: {context.Message.OrderId}");
            await context.Publish(new PaymentProcessed(context.Message.OrderId));
        }
    }
}