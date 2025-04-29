using BankAccount.Proyecciones.Worker;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Weasel.Core;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddMarten(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("MartenEventStore");
        options.Connection(connectionString);
        options.AutoCreateSchemaObjects = AutoCreate.All;

        options.Projections.Add<EstadoCuentaProjection>(ProjectionLifecycle.Async);
        options.Projections.Add<MovimientoCuentaProjection>(ProjectionLifecycle.Async);
        options.Projections.Add<SaldoTotalClienteProjection>(ProjectionLifecycle.Async);
    })
    .AddAsyncDaemon(DaemonMode.Solo);


var host = builder.Build();
await host.RunAsync();