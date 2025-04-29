using static BankAccount.Dominio.CuentaBancaria.Eventos;

namespace BankAccount.Dominio.CuentaBancaria.Comandos;

public record CrearCuentaBancaria(Guid IdCuentaBancaria, string IdCliente);

public class CrearCuentaBancariaHandler(IEventStore eventStore) : ICommandHandler<CrearCuentaBancaria>
{
    public Task HandleAsync(CrearCuentaBancaria command)
    {
        var cuentaBancariaCreada = new CuentaBancariaCreada(command.IdCuentaBancaria, command.IdCliente);

        eventStore.AppendEvent(command.IdCuentaBancaria, cuentaBancariaCreada);

        return Task.CompletedTask;
    }
}