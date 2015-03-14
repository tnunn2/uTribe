using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.Common.Interfaces
{
    public interface IUser : IDBRepositoryObject
    {

        List<string> Channels { get; }

        new Guid ID
        {
            get;
        }

        string Token { get; }
        string InvitesChannel { get; set; }
    }
}
