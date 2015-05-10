using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.Messaging
{
    public interface IRTFStringBuilder
    {
        string MakeCreateString(string tname);
        string MakeAuthString(List<string> tableNames, string userToken);
        string MakeInviteString(string userToken, string userTableName, string eventTableName, string invitedBy);
    }
}
