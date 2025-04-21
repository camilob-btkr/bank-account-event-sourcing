using BankAccount.Dominio.CuentaBancaria.Comandos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount.Dominio.CuentaBancaria
{
    public class CuentaBancaria : AggregateRoot
    {
        public Guid Id { get; private set; }
        public decimal Saldo { get; private set; }

        public void Apply(Eventos.CuentaBancariaCreada @event)
        {
            Id = @event.IdCuentaBancaria;
        }

        public void Apply(Eventos.DineroDepositado @event)
        {
            Saldo += @event.Monto;
        }

        public void Apply(Eventos.DineroRetirado @event) { 
            Saldo -= @event.MontoRetirado;
        }
    }
}
