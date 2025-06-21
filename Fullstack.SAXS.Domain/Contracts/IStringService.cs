using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.SAXS.Persistence.Contracts
{
    public interface IStringService
    {
        string GetCsvFolder();
        string GetGraphUriPath();
    }
}
