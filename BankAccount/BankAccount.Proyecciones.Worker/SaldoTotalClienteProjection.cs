using BankAccount.Dominio.CuentaBancaria;
using BankAccount.Modelos.Lectura;
using Marten.Events.Projections;
using static BankAccount.Dominio.CuentaBancaria.Eventos;

namespace BankAccount.Proyecciones.Worker;

public class SaldoTotalClienteProjection : MultiStreamProjection<SaldoTotalCliente, string>
{
    public SaldoTotalClienteProjection()
    {
        Identity<CuentaBancariaCreada>(e => e.IdCliente);
        Identity<DineroDepositado>(e => e.IdCliente);
        Identity<DineroRetirado>(e => e.IdCliente);
    }

    public void Apply(SaldoTotalCliente saldo, Eventos.CuentaBancariaCreada @event)
    {
    }

    public void Apply(SaldoTotalCliente saldo, Eventos.DineroDepositado @event)
    {
        saldo.SaldoTotal += @event.Monto;
    }

    public void Apply(SaldoTotalCliente saldo, Eventos.DineroRetirado @event)
    {
        saldo.SaldoTotal -= @event.MontoRetirado;
    }
}