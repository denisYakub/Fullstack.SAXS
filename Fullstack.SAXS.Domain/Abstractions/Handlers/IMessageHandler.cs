namespace Fullstack.SAXS.Domain.Abstractions.Handlers
{
    public interface IMessageHandler<in TMessage>
    {
        Task HandleAsync(TMessage message, CancellationToken cancellationToken);
    }
}
