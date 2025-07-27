namespace Fullstack.SAXS.Application.Contracts
{
    public interface IProducer<in TMessage> : IDisposable
    {
        Task ProduceAsync(TMessage message, CancellationToken cancellationToken = default);
    }
}
