using InventoryService;
using MassTransit;
using SagaECommerce.InventoryService.Consumer;

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

        cfg.ReceiveEndpoint("inventory-service", e =>
        {
            e.ConfigureConsumers(context);
        });
    });
});

var host = builder.Build();
host.Run();
