﻿using System;
using System.Collections.Generic;
using urTribeWebAPI.Common;
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
        private readonly IUserRepository _repository; 
        #endregion

        #region Properties
        private IUserRepository UsrRepository
        {
            get
            {
                return _repository;
            }
        }
        #endregion

        #region Constructors
        public UserFacade ()
        {
                var factory = RepositoryFactory.Instance;
                _repository = factory.Create<IUserRepository>();
        }
        #endregion

        #region Public Methods
        public Guid CreateUser (IUser user)
        {
            try
            {
                if (!ValidateUserRequiredFieldsAreFilled(user, true))
                    throw new Exception("Invalid IUser object passed to the CreateUser method ");
 
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

        public void UpdateUser (IUser user)
        {
            try
            {
                UsrRepository.Update(user);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "UpdateUser", Exception = ex };
            }
        }

        public IUser FindUser (Guid userId)
        {
            try
            {
                var userList = UsrRepository.Find(user => user.ID == userId);

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
                if (usrId.Equals(friendId))
                    throw new UserContactAssociationException("UserId and ContactId are the same.");

                var factory = RepositoryFactory.Instance;
                IUserRepository userRepository = factory.Create<IUserRepository>();
                userRepository.AddToContactList(usrId, friendId);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "AddContact", Exception = ex };
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

        public IEnumerable<IEvent> RetrieveEventsByAttendanceStatus(Guid usrId, EventAttendantsStatus status)
        {
            try
            {
                IEnumerable<IEvent> evtList = UsrRepository.RetrieveAllEventsByStatus(usrId, status);
                return evtList;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "RetrieveContacts", Exception = ex };
                return null;
            }

        }

        public void RemoveContact (Guid usrId, Guid friendId)
        {
            try
            {
                    UsrRepository.RemoveContact(usrId, friendId);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "RemoveContract", Exception = ex };
            }
        }

        public Guid CreateEvent (IUser user, IEvent evt)
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IEventRepository eventRepository = factory.Create<IEventRepository>();

                ((ScheduledEvent)evt).ID = Guid.NewGuid();
                eventRepository.Add(user, evt);

                return evt.ID;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "CreateEvent", Exception = ex };
                return new Guid("99999999-9999-9999-9999-999999999999");
            }
        }

        public void Dispose()
        {
        }
        #endregion

        #region Private Method
        private bool ValidateUserRequiredFieldsAreFilled (IUser usr, bool isNew)
        {
            bool passed = true;

            passed &= (usr != null);
            passed &= (usr.ID != new Guid("99999999-9999-9999-9999-999999999999"));
            passed &= (isNew ^ (usr.ID != new Guid("00000000-0000-0000-0000-000000000000")));
            passed &= usr.Name != string.Empty && usr.Name != null;

            return passed;
        }
        #endregion

    }
}
