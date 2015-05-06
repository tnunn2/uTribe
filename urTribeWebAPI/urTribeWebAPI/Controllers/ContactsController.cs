using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Common.Concrete;
using urTribeWebAPI.BAL;



namespace urTribeWebAPI.Controllers
{
    public class ContactsController : ApiController
    {
        public IEnumerable<IUser> Get(Guid userId)
        {
            using (UserFacade facade = new UserFacade())
            {
                IEnumerable<IUser> friends = facade.RetrieveContacts(userId);
                return friends;
            }
        }

        public bool Post(Guid userId, Guid contactId)
        {
            bool done = false;
            using (UserFacade facade = new UserFacade())
            {
                facade.AddContact(userId, contactId);
                done = true;
            }
            return done;
        }

        public bool Delete(Guid userId, Guid contactId)
        {
            bool done = false;
            using (UserFacade facade = new UserFacade())
            {
                facade.RemoveContact(userId, contactId);
                done = true;
            }
            return done;
        }
    }
}
