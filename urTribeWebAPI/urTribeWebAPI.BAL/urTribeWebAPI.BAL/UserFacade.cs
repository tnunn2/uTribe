using System;
using System.Collections.Generic;
using urTribeWebAPI.Common;
using urTribeWebAPI.Common.Logging;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.Messaging;

namespace urTribeWebAPI.BAL
{
    public class UserFacade : IDisposable
    {

        #region Member Variables
        private readonly IUserRepository _usrrepository;
        private readonly RepositoryFactory _factory;
        private readonly RealTimeBroker_N _realTimeBroker;
        #endregion

        #region Properties
        private IUserRepository UsrRepository
        {
            get
            {
                return _usrrepository;
            }
        }
        private RepositoryFactory Factory
        {
            get
            {
                return _factory;
            }
        }
        private RealTimeBroker_N RTBroker
        {
            get
            {
                return _realTimeBroker;
            }
        }
        #endregion

        #region Constructors
        public UserFacade ()
        {
                _factory = RepositoryFactory.Instance;
                _usrrepository = _factory.Create<IUserRepository>();

                _realTimeBroker = new RealTimeBroker_N ();
        }
        #endregion

        #region Public Methods
        public Guid CreateUser (IUser user)
        {

           if (!ValidateUserRequiredFieldsAreFilled(user, true))
                throw new UserException("Invalid IUser object passed to the CreateUser method ");

            try
            {
                ((User)user).ID = Guid.NewGuid();
                UsrRepository.Add(user);
                return user.ID;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "CreateUser", Exception = ex };
                throw;
            }
        }

        public void UpdateUser (IUser user)
        {
            try
            {
                if (!ValidateUserRequiredFieldsAreFilled (user, false))
                    throw new UserException("Invalid IUser object passed to the CreateUser method ");

                UsrRepository.Update(user);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "UpdateUser", Exception = ex };
                throw;
            }
        }

        public IUser FindUser (Guid userId)
        {
            if (userId == new Guid("99999999-9999-9999-9999-999999999999") || userId == new Guid())
                throw new InvalidUserIdException(userId);

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
                throw;
            }
        }

        public void AddContact(Guid usrId, Guid friendId)
        {
            if (usrId == new Guid("99999999-9999-9999-9999-999999999999") || usrId == new Guid())
                throw new InvalidUserIdException(usrId);

            if (friendId == new Guid("99999999-9999-9999-9999-999999999999") || friendId == new Guid())
                throw new InvalidUserIdException(friendId);

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
                throw;
            }
        }

        public IEnumerable<IUser> RetrieveContacts(Guid userId)
        {
            if (userId == new Guid("99999999-9999-9999-9999-999999999999") || userId == new Guid())
                throw new InvalidUserIdException(userId);

            try
            {
                IEnumerable<IUser> userList = UsrRepository.RetrieveContacts(userId);
                return userList;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "RetrieveContacts", Exception = ex };
                throw;
            }
        }

        public IEnumerable<IEvent> RetrieveEventsByAttendanceStatus(Guid usrId, EventAttendantsStatus status)
        {
            if (usrId == new Guid("99999999-9999-9999-9999-999999999999") || usrId == new Guid())
                throw new InvalidUserIdException(usrId);

            try
            {
                IEnumerable<IEvent> evtList = UsrRepository.RetrieveAllEventsByStatus(usrId, status);
                return evtList;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "RetrieveContacts", Exception = ex };
                throw;
            }

        }

        public void RemoveContact (Guid usrId, Guid friendId)
        {
            if (usrId == new Guid("99999999-9999-9999-9999-999999999999") || usrId == new Guid())
                throw new InvalidUserIdException(usrId);

            if (friendId == new Guid("99999999-9999-9999-9999-999999999999") || friendId == new Guid())
                throw new InvalidUserIdException(friendId);

            try
            {
                    UsrRepository.RemoveContact(usrId, friendId);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "UserFacade", FaultMethod = "RemoveContract", Exception = ex };
                throw;
            }
        }

        public Guid CreateEvent(Guid userId, IEvent evt)
        {
            if (userId == new Guid("99999999-9999-9999-9999-999999999999") || userId == new Guid())
                throw new InvalidUserIdException(userId);

            if (evt == null)
                throw new EventException("A null was passed as a event");

            try
            {
                IUser user = FindUser(userId);
                if (user == null)
                    throw new UserException("User doesnot exist");

                ((ScheduledEvent)evt).ID = Guid.NewGuid();

                var evtRepository = Factory.Create<IEventRepository>();
                evtRepository.Add(user, evt);

                //Here Add Code to insert create a new event in the real time framework.
                var result = RTBroker.CreateEventChannel(evt.ID.ToString(), user);

                if (result.ok())
                {
                    var eventList = RetrieveEventsByAttendanceStatus(userId, EventAttendantsStatus.All);
                    var convertedEventList = RTBroker.ConvertEventToTableName(eventList);
                    result = RTBroker.AuthUser(convertedEventList, user.Token);
                }

                return ((ScheduledEvent)evt).ID;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "CreateEvent", Exception = ex };
                throw;
            }
        }

        public void CancelEvent (Guid userId, Guid eventId)
        {
            if (userId == new Guid("99999999-9999-9999-9999-999999999999") || userId == new Guid())
                throw new InvalidUserIdException(userId);

            if (eventId == new Guid("99999999-9999-9999-9999-999999999999") || eventId == new Guid())
                throw new InvalidEventIdException(eventId);

            try
            {
                using (EventFacade eventFacade = new EventFacade())
                {
                    IEvent evt = eventFacade.FindEvent(eventId);

                    var evtRepository = Factory.Create<IEventRepository>();
                    if (evtRepository.Guest(evt, userId))
                        return;
                    if (evt.Active == false)
                        return;

                    evt.Active = false;
                    eventFacade.UpdateEvent(userId, evt);

                    //Potientially Add Code for Messaging (RTF).  However, The eventFacade.UpdateEvent may have code for RTF.

                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "CancelEvent", Exception = ex };
                throw;
            }
        }

        public void Dispose()
        {
        }
        #endregion

        #region Private Method
        private bool ValidateUserRequiredFieldsAreFilled (IUser usr, bool isNew)
        {
            if (usr == null)
                return false;

            bool passed = true;

            passed &= (usr.ID != new Guid("99999999-9999-9999-9999-999999999999"));
            passed &= (isNew ^ (usr.ID != new Guid("00000000-0000-0000-0000-000000000000")));
            passed &= usr.Name != string.Empty && usr.Name != null;

            return passed;
        }

        #endregion

    }
}
