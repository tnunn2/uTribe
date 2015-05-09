using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Messaging
{
    public class Policy
    {
        public DBPolicy database { get; set; }
        public Dictionary<String, TablePolicy> tables { get; set; }
    }
}
