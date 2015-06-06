using System;
using System.Collections.Generic;
using System.Linq;
using urTribeWebAPI.Common;
using urTribeWebAPI.Common.Logging;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.Messaging;

namespace urTribeWebAPI.BAL
{
    public class EventFacade : IDisposable
    {
        #region Member Variables
        private readonly IEventRepository _repository;
        private readonly RealTimeBroker_N _realTimeBroker;
        #endregion

        #region Properties
        private IEventRepository EvtRepository
        {
            get
            {
                return _repository;
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
        public EventFacade()
        {
            var factory = RepositoryFactory.Instance;
            _repository = factory.Create<IEventRepository>();

            _realTimeBroker = new RealTimeBroker_N();
        }

        public EventFacade(IMessageConnect RTFConnection)
        {
            var factory = RepositoryFactory.Instance;
            _repository = factory.Create<IEventRepository>();

            _realTimeBroker = new RealTimeBroker_N(RTFConnection);
        }
        #endregion

        #region Public Methods
        public void UpdateEvent(Guid userId, IEvent evt)
        {
            try
            {
                if (!ValidateEventRequiredFields(evt, false))
                    throw new InvalidIEventObjectException();

                if (userId != EvtRepository.Owner(evt))
                    throw new UserNotOwnerException();

                EvtRepository.Update(evt);

                //Add code here for Messaging (RTF)
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "UpdateEvent", Exception = ex };
                throw;
            }
        }
        public IEvent EventDetail (Guid userId, Guid eventId)
        {
            IEvent evt = FindEvent(eventId);

            if (!EvtRepository.Guest(evt, userId) && (userId != EvtRepository.Owner(evt)))
                throw new EventException("User is not the owner of the event, nor an invited guest.");
            else
                return evt;
        }
        public IEvent FindEvent(Guid eventId)
        {
            try
            {
                if (eventId == new Guid("99999999-9999-9999-9999-999999999999") || eventId == new Guid())
                   throw new InvalidEventIdException(eventId);

                var eventList = EvtRepository.Find(evt => evt.ID == eventId);

                foreach (IEvent evt in eventList)
                    return evt;
                return null;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "FindEvent", Exception = ex };
                throw;
            }
        }
        public void AddContactsToEvent(Guid userId, Guid eventId, List<Guid> contactList)
        {
            try
            {
                IEvent evt = FindEvent(eventId);

                var ownerId = EvtRepository.Owner(evt);
                if (userId != ownerId)
                    throw new EventException("Only the owner can add guest to the event.");

                using (UserFacade userFacade = new UserFacade(_realTimeBroker))
                {

                    var owner = userFacade.FindUser(ownerId);
                    if (owner.UserChannel == null)
                    {
                        owner.UserChannel = _realTimeBroker.ConvertUserToTableName(owner);
                    }

                    foreach (var contactId in contactList)
                    {
                        if (EvtRepository.Guest(evt, contactId))
                            continue;

                        IUser contact = userFacade.FindUser(contactId);
                        if (contact != null)
                        {
                            if (contact.UserChannel == null)
                            {
                                contact.UserChannel = _realTimeBroker.ConvertUserToTableName(contact);
                            }
                            EvtRepository.LinkToEvent(contact, evt);

                            //Here Add code for Real Time Framework
                            var eventList = userFacade.RetrieveEventsByAttendanceStatus(contactId, EventAttendantsStatus.All);
                            var channelsList = new List<string>() { contact.UserChannel };
                            channelsList.AddRange(eventList.Select(e => RTBroker.ConvertEventToTableName(e)));
                            RTBroker.AuthUser(channelsList, contact.Token);
                            RTBroker.SendInvite(contact, evt.ID.ToString(), owner.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "AddContactsToEvent", Exception = ex };
                throw;
            }
        }


        public void ChangeContactAttendanceStatus(Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            try
            {
                if (attendStatus == EventAttendantsStatus.All || attendStatus == EventAttendantsStatus.Cancel)
                    throw new InvalidEventStatusException(attendStatus);

                IEvent evt = FindEvent(eventId);

                if (evt == null)
                    throw new EventException ("Specified event cannot be found"); 

                if (!EvtRepository.Guest(evt, userId))
                    throw new EventException("Specified user is not an invited guest:"+userId.ToString());

                EvtRepository.ChangeUserAttendStatus(userId, eventId, attendStatus);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "ChangeContactAttendanceStatus", Exception = ex };
                throw;
            }
        }
        public IEnumerable<IUser> InviteesByStatus(Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            try
            {
                if (userId == new Guid("99999999-9999-9999-9999-999999999999") || userId == new Guid())
                    throw new InvalidEventIdException(string.Format("Invalid UserId passed to the InviteesByStatus method: {0} ", userId.ToString()));

                if (eventId == new Guid("99999999-9999-9999-9999-999999999999") || eventId == new Guid())
                    throw new InvalidEventIdException(string.Format("Invalid eventId passed to the InviteesByStatus method: {0} ", eventId.ToString()));

                IEvent evt = FindEvent(eventId);

                if (!EvtRepository.Guest(evt, userId) && (userId != EvtRepository.Owner(evt)))
                    throw new EventException("User is not the owner of the event, nor an invited guest.");

                var results = EvtRepository.AttendingByStatus(eventId, attendStatus);
                return results;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "EventAttendeesByStatus", Exception = ex };
                throw;
            }
        }
        public void Dispose()
        {
        }
        #endregion

        #region Private Methods

        private bool ValidateEventRequiredFields(IEvent evt, bool isNew)
        {
            if (evt == null)
                return false;

            bool passed = true;
            passed &= (evt.ID != new Guid("99999999-9999-9999-9999-999999999999"));
            passed &= (isNew ^ (evt.ID != new Guid("00000000-0000-0000-0000-000000000000")));
            passed &= (evt.Time != null && evt.Time != string.Empty);
            return passed;
        }
        #endregion
    }
}
