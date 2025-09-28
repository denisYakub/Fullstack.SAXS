using Fullstack.SAXS.Domain.Dtos;

namespace Fullstack.SAXS.Domain.Abstractions.Services
{
    public interface IIntenceService
    {
        Task<string> CreateIntensOptGraphAsync(IntensityCreateDTO dto);
    }
}
