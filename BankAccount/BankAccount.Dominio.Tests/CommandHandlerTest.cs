using FluentAssertions;

namespace BankAccount.Dominio.Tests;

public abstract class CommandHandlerTest<TCommand>()
{
    protected readonly Guid _aggregateId = Guid.NewGuid();

    protected TestStore eventStore = new();
    
    protected abstract ICommandHandler<TCommand> Handler { get; }

    protected void Given(Guid aggregateId, params object[] events)
    {
        eventStore.AppendPreviosEvents(aggregateId, events);
    }

    protected void Given(params object[] events)
    {
        Given(_aggregateId, events);
    }

    protected void When(TCommand command)
    {
        Handler.HandleAsync(command).GetAwaiter().GetResult();
    }

    protected void Then(params object[] expectedEvents)
    {
        Then(_aggregateId, expectedEvents);
    }

    protected void Then(Guid aggregateId, params object[] expectedEvents)
    {
        var newEvents = eventStore.GetNewEvents(aggregateId).ToList();

        for (var i = 0; i < newEvents.Count; i++)
        {
            newEvents[i].Should().BeOfType(expectedEvents[i].GetType());
            try
            {
                newEvents[i].Should().BeEquivalentTo(expectedEvents[i]);
            }
            catch (InvalidOperationException e)
            {
                // Empty event with matching type is OK. This means that the event class
                //  has no properties. If the types match in this situation, the correct
                // event has been appended. So we should ignore this exception.
                if (!e.Message.StartsWith("No members were found for comparison."))
                    throw;
            }
        }
    }

    
}