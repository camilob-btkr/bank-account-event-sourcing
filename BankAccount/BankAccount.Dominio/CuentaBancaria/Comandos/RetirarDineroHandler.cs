

namespace BankAccount.Dominio.CuentaBancaria.Comandos;

public record RetirarDinero(Guid IdCuentaBancaria, decimal Monto);

public class RetirarDineroHandler(IEventStore eventStore) : ICommandHandler<RetirarDinero>
{
    public async Task HandleAsync(RetirarDinero command)
    {
        var cuentaBancaria = await eventStore.GetAggregateRootAsync<CuentaBancaria>(command.IdCuentaBancaria);
        if (cuentaBancaria is null)
            throw new InvalidOperationException("Cuenta no existe");
        if (cuentaBancaria.Saldo < command.Monto)
            throw new InvalidOperationException("Saldo Insuficiente");

        decimal saldoDespuesRetiro = cuentaBancaria.Saldo - command.Monto;
        var eventoDineroRetirado = new Eventos.DineroRetirado(command.IdCuentaBancaria, command.Monto, saldoDespuesRetiro);
        eventStore.AppendEvent(command.IdCuentaBancaria, eventoDineroRetirado);
    }
}

