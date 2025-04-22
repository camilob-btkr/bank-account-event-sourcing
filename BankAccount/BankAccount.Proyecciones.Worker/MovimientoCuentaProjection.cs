using BankAccount.Dominio.CuentaBancaria;
using BankAccount.Modelos.Lectura;
using Marten;
using Marten.Events;
using Marten.Events.Projections;

namespace BankAccount.Proyecciones.Worker
{
    public class MovimientoCuentaProjection : EventProjection
    {
        public void Project(IEvent<Eventos.DineroDepositado> @event, IDocumentOperations ops)
        {
            ops.Store(new MovimientoCuenta
            {
                Id = Guid.NewGuid(),
                IdCuenta = @event.Data.IdCuentaBancaria,
                Fecha = @event.Timestamp.DateTime, // o del metadata
                Monto = @event.Data.Monto,
                Saldo = @event.Data.Saldo,
                Tipo = "Deposito"
            });
        }

        public void Project(IEvent<Eventos.DineroRetirado> @event, IDocumentOperations ops)
        {
            ops.Store(new MovimientoCuenta
            {
                Id = Guid.NewGuid(),
                IdCuenta = @event.Data.IdCuentaBancaria,
                Fecha = @event.Timestamp.DateTime,
                Monto = -@event.Data.MontoRetirado,
                Saldo = @event.Data.Saldo,
                Tipo = "Retiro"
            });
        }
    }
}