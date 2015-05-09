using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Messaging
{
    public class PutItemQuery
    {
        public string applicationKey { get; set; }
        public string privateKey { get; set; }
        public string authenticationToken { get; set; }
        public string table { get; set; }
        public Dictionary<String, String> item { get; set; }
    }
}
