namespace BankAccount.Dominio.CuentaBancaria.Consultas;

public record ObtenerBalance(Guid IdCuentaBancaria);

public class ObtenerBalanceHandler(IEventStore eventStore)
{
    public Task<CuentaBancaria?> HandleAsync(ObtenerBalance command)
    {
        return eventStore.GetAggregateRootAsync<CuentaBancaria>(command.IdCuentaBancaria);
    }
}