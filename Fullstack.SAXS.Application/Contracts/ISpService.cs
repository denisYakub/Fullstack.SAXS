namespace Fullstack.SAXS.Application.Contracts
{
    public interface ISpService
    {
        string Get(Guid id);
        string GetAll(string userId = null);
        byte[] GetAtoms(Guid id);
    }
}
