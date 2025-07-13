using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.SAXS.Application.Contracts
{
    public interface IEventPublisher
    {
        Task PublishAsync(string topic, string message);
    }
}
