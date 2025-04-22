using BankAccount.Dominio.CuentaBancaria;
using BankAccount.Dominio.CuentaBancaria.Comandos;
using FluentAssertions;

namespace BankAccount.Dominio.Tests.CuentaBancaria;

public class DepositarDineroSpecifications : CommandHandlerTest<DepositarDinero>
{
    protected override ICommandHandler<DepositarDinero> Handler => new DepositarDineroHandler(eventStore);

    [Fact]
    public void Cuando_deposita_dinero_en_una_cuenta_que_no_existe_debe_generar_un_InvalidOperationException()
    {
        var caller = () => When(new DepositarDinero(_aggregateId, 1000));

        caller.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Cuando_deposita_dinero_en_una_cuenta_que_existe_debe_emitir_un_evento_DineroDepositado()
    {
        Given(new Eventos.CuentaBancariaCreada(_aggregateId));

        When(new DepositarDinero(_aggregateId, 20_000));

        Then(new Eventos.DineroDepositado(_aggregateId, 20_000, 20_000));
    }
}