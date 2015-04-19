using System;
using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Common.Logging;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.Common.Concrete;


namespace urTribeWebAPI.BAL
{
    public class UserFacade : IDisposable
    {

        #region Member Variables
        #endregion

        #region Properties
        private IUserRepository UsrRepository
        {
            get
            {
                var factory = RepositoryFactory.Instance;
                return factory.Create<IUserRepository>();
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public Guid CreateUser (IUser user)
        {
            try
            {
                ((User)user).ID = Guid.NewGuid();
                UsrRepository.Add(user);
                return user.ID;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "CreateUser", Exception = ex };
                return new Guid("99999999-9999-9999-9999-999999999999");
            }
        }

        public IUser FindUser (Guid userId)
        {
            try
            {
                var userList = UsrRepository.Find(usr => usr.ID == userId);

                foreach (IUser usr in userList)
                    return usr;
                return null;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "FindUser", Exception = ex };
                return null;
            }
        }

        public void AddContact(Guid usrId, Guid friendId)
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IUserRepository userRepository = factory.Create<IUserRepository>();
                userRepository.AddToContactList(usrId, friendId);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "AddContact", Exception = ex };
            }
        }

        public void AddContactToGroup (Guid usrId, Guid contactID, Guid groupId)
        {
            try
            {
                UsrRepository.AddFriendToGroup(usrId, contactID, groupId);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "AddContactToGroup", Exception = ex };
            }
        }

        public IEnumerable<IUser> RetrieveContacts(Guid userId)
        {
            try
            {
                IEnumerable<IUser> userList = UsrRepository.RetrieveContacts(userId);
                return userList;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "RetrieveContacts", Exception = ex };
                return null;
            }
        }

        public void RemoveContact (Guid usrId, Guid friendId, Guid groupId)
        {
            try
            {
                if (groupId == null)
                    UsrRepository.RemoveContact(usrId, friendId);
                else
                    UsrRepository.RemoveContactFromGroup(usrId, friendId, groupId);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "RemoveContract", Exception = ex };
            }
        }

        public void Dispose()
        {
        }
        #endregion

        #region Private Method
        #endregion

    }
}
