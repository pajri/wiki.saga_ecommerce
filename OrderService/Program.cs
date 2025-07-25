using MassTransit;
using SagaECommerce.OrderService.Sagas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddSagaStateMachine<OrderStateMachine, OrderState>()
        .InMemoryRepository();

    cfg.UsingRabbitMq((context, cfgRabbit) =>
    {
        cfgRabbit.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfgRabbit.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();