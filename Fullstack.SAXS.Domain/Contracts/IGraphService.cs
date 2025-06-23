using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.SAXS.Domain.Contracts
{
    public interface IGraphService
    {
        Task<string> GetHtmlPageAsync(float[] x, float[] y);
    }
}
