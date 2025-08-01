using Fullstack.SAXS.Domain.Dtos;

namespace Fullstack.SAXS.Application.Contracts
{
    public interface IIntenceService
    {
        Task<string> CreateIntensOptGraphAsync(IntensityCreateDTO dto);
    }
}
