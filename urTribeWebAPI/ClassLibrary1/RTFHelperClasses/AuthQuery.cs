using System.Collections.Generic;

namespace urTribeWebAPI.Messaging
{
    public class AuthQuery
    {
        public string applicationKey { get; set; }
        public string privateKey { get; set; }
        public string authenticationToken { get; set; }
        public List<string> roles { get; set; }
        public long timeout { get; set; }
        public Policy policies { get; set; }
    }
}
