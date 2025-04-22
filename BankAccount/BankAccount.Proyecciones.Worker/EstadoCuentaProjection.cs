using BankAccount.Dominio.CuentaBancaria;
using BankAccount.Modelos.Lectura;
using Marten.Events.Aggregation;

namespace BankAccount.Proyecciones.Worker;

public class EstadoCuentaProjection : SingleStreamProjection<EstadoCuenta>
{
    public static EstadoCuenta Create(Eventos.CuentaBancariaCreada @event)
    {
        return new EstadoCuenta
        {
            Id = @event.IdCuentaBancaria,
            SaldoActual = 0
        };
    }

    public void Apply(Eventos.DineroDepositado @event, EstadoCuenta estado)
    {
        estado.SaldoActual += @event.Monto;
    }

    public void Apply(Eventos.DineroRetirado @event, EstadoCuenta estado)
    {
        estado.SaldoActual = @event.Saldo;
    }
}