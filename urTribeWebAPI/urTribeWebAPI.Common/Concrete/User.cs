using System;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Common.Concrete
{
    public class User : IUser
    {
        public Guid ID
        {
            get;
            set;
        }
    }
}
