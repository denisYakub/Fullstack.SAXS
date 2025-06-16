using Fullstack.SAXS.Server.API.Dtos;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SAXS.Server.Application.Interfaces
{
    public interface ISysService
    {
        void Create(string? userId, CreateSysRequest request);
        JsonResult Get(Guid id);
        IHtmlContent CreatePhiGraf(Guid id, int layersNum);
        IHtmlContent CreateIntensOptGraf(Guid id, CreateIntensOptRequest request);
    }
}
