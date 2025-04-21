using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankAccount.Dominio.CuentaBancaria.Eventos;

namespace BankAccount.Dominio.CuentaBancaria.Comandos;

public record CrearCuentaBancaria(Guid IdCuentaBancaria);


public class CrearCuentaBancariaHandler(IEventStore eventStore) : ICommandHandler<CrearCuentaBancaria>
{
    public Task HandleAsync(CrearCuentaBancaria command)
    {
        var cuentaBancariaCreada = new CuentaBancariaCreada(command.IdCuentaBancaria);

        eventStore.AppendEvent(command.IdCuentaBancaria, cuentaBancariaCreada);

        return Task.CompletedTask;
    }
}
