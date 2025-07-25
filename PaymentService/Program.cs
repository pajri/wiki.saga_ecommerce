using MassTransit;
using PaymentService;
using SagaECommerce.PaymentService.Consumer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly);
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("payment-service", e =>
        {
            e.ConfigureConsumers(context);
        });
    });
});

var host = builder.Build();
host.Run();
