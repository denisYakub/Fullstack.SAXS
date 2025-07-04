using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Models
{
    public record CreateQIData(
        double QMin, double QMax, 
        int QNum, StepTypes StepType
    );
}
