using BankAccount.API;
using BankAccount.Dominio;
using BankAccount.Dominio.CuentaBancaria;
using BankAccount.Dominio.CuentaBancaria.Comandos;
using BankAccount.Dominio.CuentaBancaria.Consultas;
using BankAccount.EventStore;
using FastExpressionCompiler;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Weasel.Core;
using Wolverine;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();
var martenConnectionString = builder.Configuration.GetConnectionString("MartenEventStore") ??
                             throw new ArgumentNullException(
                                 "La cadena de conexión 'MartenEventStore' no está configurada.");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .AddHealthChecks()
    .AddNpgSql(martenConnectionString);

builder.UseWolverine(options =>
    {
        options.Discovery.IncludeAssembly(typeof(IEventStore).Assembly);
        options.Services.AddMartenConfiguration(martenConnectionString,
            isDevelopment).IntegrateWithWolverine();
        options.Policies.AutoApplyTransactions();
        options.Durability.Mode = DurabilityMode.MediatorOnly;
    }
);

builder.Services.AddMartenEventStore();
builder.Services.AddScoped<ICommandRouter, WolverineCommandRouter>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseHealthChecks("/health");

app.MapPost("/CuentasBancarias", async (ICommandRouter router) =>
{
    await router.InvokeAsync(new CrearCuentaBancaria(Guid.CreateVersion7()));
});

app.MapPost("/CuentasBancarias/{idCuentaBancaria}/Transacciones/Depositos", async (Guid idCuentaBancaria, TransaccionRequest solicitud, ICommandRouter router) =>
{
    try
    {
        var comando = new DepositarDinero(idCuentaBancaria, solicitud.monto);
        await router.InvokeAsync(comando);
        return Results.Ok();
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(ex.Message);
    }
    
});

app.MapPost("/CuentasBancarias/{idCuentaBancaria}/Transacciones/Retiros", async (Guid idCuentaBancaria, TransaccionRequest solicitud, ICommandRouter router) =>
{
    try
    {
        var comando = new RetirarDinero(idCuentaBancaria, solicitud.monto);
        await router.InvokeAsync(comando);
        return Results.Ok();
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/CuentasBancarias/{idCuentaBancaria}", async (Guid idCuentaBancaria, IMessageBus router) =>
{
    var cuentaBancaria = await router.InvokeAsync<CuentaBancaria>(new ObtenerBalance(idCuentaBancaria));
    if(cuentaBancaria is null) return Results.NotFound("Cuenta bancaria no encontrada");
    
    return Results.Ok(cuentaBancaria);
});

app.Run();

public record TransaccionRequest(decimal monto);
