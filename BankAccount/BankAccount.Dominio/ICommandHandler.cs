namespace BankAccount.Dominio;

public interface ICommandHandler<TCommand>
{
    Task HandleAsync(TCommand command);
}