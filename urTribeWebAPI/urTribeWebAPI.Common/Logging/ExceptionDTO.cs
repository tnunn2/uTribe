using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.Common.Logging
{
    public class ExceptionDTO
    {
        public string FaultClass { get; set; }
        public string FaultMethod { get; set; }
        public Exception Exception { get; set; }
    }
}
