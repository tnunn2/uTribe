using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.BAL;

namespace urTribeWebAPI.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public IUser Users(Guid userId)
        {
            using (UserFacade facade = new UserFacade())
            {
                IUser user = facade.FindUser(userId);
                return user;
            }
        }

        [HttpPut]
        public Guid Users(IUser user)
        {
            using (UserFacade facade = new UserFacade())
            {
                Guid userId = facade.CreateUser(user);
                return userId;
            }
        }

        [HttpGet]
        public IEnumerable<IUser> Contacts (Guid userId)
        {
            using (UserFacade facade = new UserFacade())
            {
                IEnumerable<IUser> friends = facade.RetrieveContacts(userId);
                return friends;
            }
        }

        [HttpPut]
        public void Contacts(Guid userId, Guid contactId)
        {
            using (UserFacade facade = new UserFacade())
            {
                facade.AddContact(userId, contactId);
            }
        }

        [HttpDelete]
        public void Contacts (Guid userId, Guid contactId, Guid groupId)
        {
            using (UserFacade facade = new UserFacade())
            {
                facade.RemoveContact(userId, contactId);
            }
        }

    }
}