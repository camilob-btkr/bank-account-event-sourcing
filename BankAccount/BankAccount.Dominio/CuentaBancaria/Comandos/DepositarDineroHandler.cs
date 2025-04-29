namespace BankAccount.Dominio.CuentaBancaria.Comandos;

public record DepositarDinero(Guid IdCuentaBancaria, decimal Monto);

//EL handler maneja logica de negocio. Por ejemplo aca se podria validar si la cuenta no esta bloqueda, para poder aplicar eventos
public class DepositarDineroHandler(IEventStore eventStore) : ICommandHandler<DepositarDinero>
{
    public async Task HandleAsync(DepositarDinero command)
    {
        var cuentaBancaria = await eventStore.GetAggregateRootAsync<CuentaBancaria>(command.IdCuentaBancaria);
        if (cuentaBancaria is null)
            throw new InvalidOperationException("Cuenta no existe");

        decimal saldoDespuesDeposito = cuentaBancaria.Saldo + command.Monto;

        var dineroDepositado =
            new Eventos.DineroDepositado(command.IdCuentaBancaria, command.Monto, saldoDespuesDeposito,
                cuentaBancaria.IdCliente);
        eventStore.AppendEvent(command.IdCuentaBancaria, dineroDepositado);
    }
}