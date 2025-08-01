namespace Fullstack.SAXS.Domain.Dtos
{
    public record DensityCreateDTO(
        Guid UserId,
        Guid AreaId,
        int LayersNum
    );
}
