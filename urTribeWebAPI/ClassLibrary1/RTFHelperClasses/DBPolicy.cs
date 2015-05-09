using System.Collections.Generic;

namespace urTribeWebAPI.Messaging
{
    public class DBPolicy
    {
        public List<string> listTables { get; set; }
        public List<string> deleteTable { get; set; }
        public bool createTable { get; set; }
        public List<string> updateTable { get; set; }
    }
}
