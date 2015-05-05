using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Common.Concrete;
using urTribeWebAPI.BAL;

namespace urTribeWebAPI.Controllers
{
    public class UsersController : ApiController
    {
        public IUser Get(Guid userId)
        {
            using (UserFacade facade = new UserFacade())
            {
                IUser user = facade.FindUser(userId); 
                return user;
            }
        }

        public Guid Post(User user)
        {
            using (UserFacade facade = new UserFacade())
            {
                Guid userId;
                if (user.ID == new Guid())
                    userId = facade.CreateUser(user);
                else
                {
                    facade.UpdateUser(user);
                    userId = user.ID;
                }
                return userId;
            }
        }      
    }
}