namespace BankAccount.Dominio.CuentaBancaria;

public class Eventos
{
    public record DineroRetirado(Guid IdCuentaBancaria, decimal MontoRetirado, decimal Saldo);

    public record DineroDepositado(Guid IdCuentaBancaria, decimal Monto);

    public record CuentaBancariaCreada(Guid IdCuentaBancaria);
}
