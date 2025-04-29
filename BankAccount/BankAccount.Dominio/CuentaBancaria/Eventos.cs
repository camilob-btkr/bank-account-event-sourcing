namespace BankAccount.Dominio.CuentaBancaria;

public class Eventos
{
    public record DineroRetirado(Guid IdCuentaBancaria, decimal MontoRetirado, decimal Saldo, string IdCliente);

    public record DineroDepositado(Guid IdCuentaBancaria, decimal Monto, decimal Saldo, string IdCliente);

    public record CuentaBancariaCreada(Guid IdCuentaBancaria, string IdCliente);
}