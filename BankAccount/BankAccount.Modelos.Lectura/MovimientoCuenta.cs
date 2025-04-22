namespace BankAccount.Modelos.Lectura;

public class MovimientoCuenta
{
    public Guid Id { get; set; }
    public Guid IdCuenta { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Monto { get; set; }
    public decimal Saldo { get; set; }
    public string Tipo { get; set; } = "";
}