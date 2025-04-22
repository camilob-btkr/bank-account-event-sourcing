namespace BankAccount.Dominio.CuentaBancaria;

public class Eventos
{
    public record DineroRetirado(Guid IdCuentaBancaria, decimal MontoRetirado, decimal Saldo);

    public record DineroDepositado(Guid IdCuentaBancaria, decimal Monto, decimal Saldo);

    public record CuentaBancariaCreada(Guid IdCuentaBancaria);
}