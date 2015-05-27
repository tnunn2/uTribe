using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace urTribeWebAPI.Messaging.RTFHelperClasses
{
    class CreationResponse
    {
        public CreationDataResponse data { get; set; }
        public ErrorResponse error { get; set; }
    }

}
