using BankAccount.Modelos.Lectura;
using Marten;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("MartenEventStore"));
    options.AutoCreateSchemaObjects = AutoCreate.None;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/estado-cuenta/{id:guid}", async (Guid id, IQuerySession session) =>
{
    var estado = await session.LoadAsync<EstadoCuenta>(id);
    return estado is not null ? Results.Ok(estado) : Results.NotFound();
});

app.MapGet("/estado-cuenta/{id:guid}/movimientos", async (Guid id, IQuerySession session) =>
{
    var movimientos = await session
        .Query<MovimientoCuenta>()
        .Where(m => m.IdCuenta == id)
        .OrderByDescending(m => m.Fecha)
        .ToListAsync();

    if (!movimientos.Any())
        return Results.NotFound("No hay movimientos para esta cuenta");

    var tabla = GenerarTablaMovimientos(movimientos);
    return Results.Text(tabla, "text/plain");
});

app.MapGet("/clientes/{idCliente}/saldo-total", async (string idCliente, IQuerySession session) =>
{
    var saldo = await session.LoadAsync<SaldoTotalCliente>(idCliente);

    if (saldo == null)
        return Results.NotFound($"No se encontró saldo para el cliente {idCliente}");

    return Results.Ok(saldo);
});

string GenerarTablaMovimientos(IEnumerable<MovimientoCuenta> movimientos)
{
    var sb = new System.Text.StringBuilder();


    sb.AppendLine($"| {"Fecha",-12} | {"Monto",15} | {"Saldo",15} | {"Tipo",-10} |");
    sb.AppendLine($"|{new string('-', 14)}|{new string('-', 17)}|{new string('-', 17)}|{new string('-', 12)}|");


    foreach (var m in movimientos)
    {
        var fecha = m.Fecha.ToString("yyyy-MM-dd");
        var monto = (m.Monto >= 0 ? "+" : "") + m.Monto.ToString("N2");
        var saldo = m.Saldo.ToString("N2");
        var tipo = m.Tipo;

        sb.AppendLine($"| {fecha,-12} | {monto,15} | {saldo,15} | {tipo,-10} |");
    }

    return sb.ToString();
}

app.Run();