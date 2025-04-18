using Microsoft.EntityFrameworkCore;
using SmartShipping.Infrastructure.Services;
using SmartShipping.Persistence;
using SmartShipping.Application.Interfaces;
using SmartShipping.Application.Features.Shipments.Commands;
using MassTransit;
using StackExchange.Redis;
using SmartShipping.Shared.Services;
using SmartShipping.Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CQRS
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateShipmentCommand).Assembly));

// Custom Services
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<ICarrierSelectorService, CarrierSelectorService>();

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis:6379, abortConnect=false"));
builder.Services.AddSingleton<RedisCacheService>();

// Rabbit Mq
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});
Console.WriteLine("Connection string:");
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

// Migrations ve seed iþlemi
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    await DataSeeder.SeedAsync(context);
}
//using (var scope = app.Services.CreateScope())
//{
//    try
//    {
//        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//        context.Database.Migrate();
//        await DataSeeder.SeedAsync(context);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Migration or seeding failed: {ex.Message}");
//        throw;
//    }
//}

// Middleware
app.UseGlobalExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
