using MassTransit;
using SagaECommerce.Contracts.Inventory.Commands;
using SagaECommerce.Contracts.Inventory.Events;
using SagaECommerce.Contracts.Order.Events;
using SagaECommerce.Contracts.Payment.Commands;
using SagaECommerce.Contracts.Payment.Events;

namespace SagaECommerce.OrderService.Sagas;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    #region constants
    public const string InventoryServiceQueueUri = "rabbitmq://localhost/inventory-service";
    public const string PaymentServiceQueueUri = "rabbitmq://localhost/payment-service";
    #endregion

    #region states
    public State OrderCreatedState { get; private set; }
    public State ReservingInventoryState { get; private set; }
    public State CommitingInventoryState { get; private set; }
    public State RollingbackInventoryState { get; private set; }
    public State ProcessingPaymentState { get; private set; }
    #endregion

    public Event<OrderCreated> OrderCreated { get; private set; }
    public Event<InventoryReserved> InventoryReserved { get; private set; }
    public Event<PaymentProcessed> PaymentProcessed { get; private set; }
    public Event<PaymentFailed> PaymentFailed { get; private set; }
    public Event<InventoryCommited> InventoryCommited { get; private set; }
    public Event<InventoryRestored> InventoryRestored { get; private set; }

    public OrderStateMachine()
    {
        //state instance registration
        InstanceState(x => x.CurrentState);

        //events registration
        Event(() => OrderCreated, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => InventoryReserved, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentProcessed, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => InventoryCommited, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => InventoryRestored, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentFailed, x => x.CorrelateById(context => context.Message.OrderId));

        //saga flow setup
        Initially(
            When(OrderCreated)
                .Then(context => Console.WriteLine("order received"))
                .Send(new Uri(InventoryServiceQueueUri), 
                            context => new ReserveInventoryCommand(context.Message.OrderId))
                .TransitionTo(ReservingInventoryState)
        );

        During(ReservingInventoryState,
            When(InventoryReserved)
                .Then(context => Console.WriteLine("inventory reserved"))
                .Send(new Uri(PaymentServiceQueueUri), 
                            context => new ProcessPaymentCommand(context.Message.OrderId))
                .TransitionTo(ProcessingPaymentState)
        );
 
        During(ProcessingPaymentState,
            When(PaymentProcessed)
                .Then(context => Console.WriteLine("payment processed"))
                .Send(new Uri(InventoryServiceQueueUri), 
                            context => new CommitInventoryCommand(context.Message.OrderId))
                .TransitionTo(CommitingInventoryState),
                
            When(PaymentFailed)
                .Then(context => Console.WriteLine("payment failed"))
                .Send(new Uri(InventoryServiceQueueUri), 
                            context => new RollbackInventoryCommand(context.Message.OrderId))
                .TransitionTo(RollingbackInventoryState)
        );

        During(RollingbackInventoryState,
            When(InventoryRestored)
            .Then(context => Console.WriteLine("inventory restored"))
            .Finalize()
        );

        During(CommitingInventoryState,
            When(InventoryCommited)
                .Then(context => Console.WriteLine("inventory commited"))
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}