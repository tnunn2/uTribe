using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.Messaging.RTFHelperClasses
{
    public class BooleanResponse
    {
        public bool data { get; set; }
        public ErrorResponse error { get; set; }
    }
}
