using BankAccount.Dominio.CuentaBancaria.Comandos;
using static BankAccount.Dominio.CuentaBancaria.Eventos;

namespace BankAccount.Dominio.Tests.CuentaBancaria
{
    public class CrearCuentaBancariaSpecifications : CommandHandlerTest<CrearCuentaBancaria>
    {
        protected override ICommandHandler<CrearCuentaBancaria> Handler => new CrearCuentaBancariaHandler(eventStore);

        [Fact]
        public void Cuando_crea_una_cuenta_bancaria_debe_emitir_evento_CuentaBancariaCreada()
        {
            When(
                new CrearCuentaBancaria(_aggregateId, "IdCliente")
            );

            Then(
                new CuentaBancariaCreada(_aggregateId, "IdCliente")
            );
        }
    }
}