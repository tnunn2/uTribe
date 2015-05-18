using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using urTribeWebAPI.Common;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Models.Response;


namespace urTribeWebAPI.Controllers
{
    public class ContactsController : ApiController
    {
        //Get a list of User Contacts
        public APIResponse Get(Guid userId)
        {

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    IEnumerable<IUser> friends = facade.RetrieveContacts(userId);

                    APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, new { Contacts = friends });
                    return response;
                }
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse(APIResponse.ReponseStatus.error, new { Error = ex.Message });
                return response;
            }

        }

        //Link to Contact to User
        public APIResponse Post(Guid userId, Guid contactId)
        {
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.AddContact(userId, contactId);
                    APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, null);
                    return response;
                }
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse(APIResponse.ReponseStatus.error, new { Error = ex.Message });
                return response;
            }
        }

        //Remove a contact
        public APIResponse Delete(Guid userId, Guid contactId)
        {
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.RemoveContact(userId, contactId);
                    APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, null);
                    return response;
                }
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse(APIResponse.ReponseStatus.error, new { Error = ex.Message });
                return response;
            }
        }
    }
}
