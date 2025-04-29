using BankAccount.Dominio.CuentaBancaria;
using BankAccount.Dominio.CuentaBancaria.Comandos;
using FluentAssertions;
using static BankAccount.Dominio.CuentaBancaria.Eventos;

namespace BankAccount.Dominio.Tests.CuentaBancaria;

public class RetirarDineroSpecifications : CommandHandlerTest<RetirarDinero>
{
    protected override ICommandHandler<RetirarDinero> Handler => new RetirarDineroHandler(eventStore);

    [Fact]
    public void Cuando_retiro_dinero_y_la_cuenta_no_existe_debe_generar_un_InvalidOperationException()
    {
        var caller = () => When(new RetirarDinero(_aggregateId, 2800));

        caller.Should().ThrowExactly<InvalidOperationException>().WithMessage("Cuenta no existe");
    }

    [Fact]
    public void
        Cuando_retiro_dinero_yla_cuenta_exite_pero_no_tiene_saldo_suficiente_debe_generar_una_excepcion_InvalidOperationException_con_mensaje_Saldo_Insuficiente()
    {
        Given(new Eventos.CuentaBancariaCreada(_aggregateId, "IdCliente"));

        var caller = () => When(new RetirarDinero(_aggregateId, 2800));

        caller.Should().ThrowExactly<InvalidOperationException>().WithMessage("Saldo Insuficiente");
    }

    [Fact]
    public void Cuando_retiro_dinero_y_la_cuenta_existe_y_tiene_saldo_suficiente_debe_emitir_un_evento_DineroRetirado()
    {
        Given(new Eventos.CuentaBancariaCreada(_aggregateId, "IdCliente"),
            new Eventos.DineroDepositado(_aggregateId, 2800, 2800, "IdCliente"));

        When(new RetirarDinero(_aggregateId, 2800));

        Then(new Eventos.DineroRetirado(_aggregateId, 2800, 0, "IdCliente"));
    }

    [Fact]
    public void
        Cuando_retiro_20_000_pesos_la_cuenta_existe_y_tengo_saldo_de_25_000_debe_emitir_un_evento_DineroRetirado_con_20_000_de_monto_y_5_000_de_saldo()
    {
        Given(new CuentaBancariaCreada(_aggregateId, "IdCliente"),
            new DineroDepositado(_aggregateId, 25_000, 25_000, "IdCliente"));

        When(new RetirarDinero(_aggregateId, 20_000));

        Then(new DineroRetirado(_aggregateId, 20_000, 5000, "IdCliente"));
    }

    [Fact]
    public void
        Cuando_hago_dos_depositos_de_20_000_y_retiro_10_000_debe_emitir_un_evento_DineroRetirado_con_saldo_30_000_y_monto_retirado_10_000()
    {
        Given(new CuentaBancariaCreada(_aggregateId, "IdCliente"),
            new DineroDepositado(_aggregateId, 20_000, 20_000, "IdCliente"),
            new DineroDepositado(_aggregateId, 20_000, 40_000, "IdCliente"));

        When(new RetirarDinero(_aggregateId, 10_000));

        Then(new DineroRetirado(_aggregateId, 10_000, 30_000, "IdCliente"));
    }

    [Fact]
    public void
        Cuando_hago_un_deposito_de_20_000_y_dos_retiros_de_5000_cada_uno_debe_emitir_un_evento_DineroRetirado_con_saldo_10_000_y_monto_retirado_5000()
    {
        Given(new CuentaBancariaCreada(_aggregateId, "IdCliente"),
            new DineroDepositado(_aggregateId, 20_000, 20_000, "IdCliente"),
            new DineroRetirado(_aggregateId, 5_000, 15_000, "IdCliente"));

        When(new RetirarDinero(_aggregateId, 5_000));

        Then(new DineroRetirado(_aggregateId, 5_000, 10_000, "IdCliente"));
    }
}