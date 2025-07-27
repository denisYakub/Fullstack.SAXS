namespace Fullstack.SAXS.Domain.Models
{
    public class SystemMessage
    {
        public required Guid UserId { get; set; }
        public required CreateSysData CreateSysData { get; set; }
    }
}
