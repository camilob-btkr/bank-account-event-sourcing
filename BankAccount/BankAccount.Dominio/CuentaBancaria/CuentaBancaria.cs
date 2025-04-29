namespace BankAccount.Dominio.CuentaBancaria
{
    public class CuentaBancaria : AggregateRoot
    {
        public Guid Id { get; private set; }
        public decimal Saldo { get; private set; }
        public string IdCliente { get; private set; }

        public void Apply(Eventos.CuentaBancariaCreada @event)
        {
            Id = @event.IdCuentaBancaria;
            IdCliente = @event.IdCliente;
        }

        public void Apply(Eventos.DineroDepositado @event)
        {
            Saldo += @event.Monto;
        }

        public void Apply(Eventos.DineroRetirado @event)
        {
            Saldo -= @event.MontoRetirado;
        }
    }
}