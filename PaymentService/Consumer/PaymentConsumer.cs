using MassTransit;
using SagaECommerce.Contracts.Payment.Commands;
using SagaECommerce.Contracts.Payment.Events;

namespace SagaECommerce.PaymentService.Consumer
{
    public class PaymentConsumer: IConsumer<ProcessPaymentCommand>
    {
        public async Task Consume(ConsumeContext<ProcessPaymentCommand> context)
        {
            bool paymentSuccess = false;

            Console.WriteLine($"[PaymentConsumer] Processing payment for OrderId: {context.Message.OrderId}");

            if (paymentSuccess)
            {
                Console.WriteLine($"[PaymentConsumer] Payment successful");
                await context.Publish(new PaymentProcessed(context.Message.OrderId));
            }
            else
            {
                Console.WriteLine($"[PaymentConsumer] Payment failed");
                await context.Publish(new PaymentFailed(context.Message.OrderId));
            }
            
        }
    }
}