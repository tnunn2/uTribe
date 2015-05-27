using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using urTribeWebAPI.Common;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Models.Response;

namespace urTribeWebAPI.Controllers
{
    public class UsersController : ApiController
    {
        //Get User data
        public APIResponse Get(Guid userId)
        {
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    IUser user = facade.FindUser(userId);
                    APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, new { User = user });
                    return response;
                }
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse(APIResponse.ReponseStatus.error, new { Error = ex.Message });
                return response;
            }
        }
        //Get User's Status
        public APIResponse Get(Guid eventId, Guid userId)
        {
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    EventAttendantsStatus status = facade.RetrieveUsersEventStatus(userId, eventId);
                    APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, new { Status = status.ToString() });
                    return response;
                }
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse(APIResponse.ReponseStatus.error, new { Error = ex.Message });
                return response;
            }
        }
        //Create User data
        //Update User data
        public APIResponse Post(User user)
        {
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    Guid userId;
                    if (user.ID == new Guid())
                    {
                        userId = facade.CreateUser(user);

                        APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, new { Token = userId });
                        return response;
                    }
                    else
                    {
                        facade.UpdateUser(user);
                        userId = user.ID;

                        APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, null);
                        return response;
                    }
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