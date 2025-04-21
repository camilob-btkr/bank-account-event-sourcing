using BankAccount.Dominio;
using Wolverine;

namespace BankAccount.API;

public class WolverineCommandRouter(IMessageBus messageBus) : ICommandRouter
{
    public Task InvokeAsync<TCommand>(TCommand command) where TCommand : class
    {
        return messageBus.InvokeAsync<TCommand>(command);
    }
}