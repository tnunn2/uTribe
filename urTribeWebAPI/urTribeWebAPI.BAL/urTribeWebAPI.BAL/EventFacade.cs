using System;
using System.Collections.Generic;
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
        #endregion

        #region Public Methods
        public Guid UpdateEvent(Guid userId, IEvent evt)
        {
            try
            {
                if (!ValidateEventRequiredFields(evt, false))
                    throw new Exception("Invalid IEvent object passed to the UpdateEvent method ");

                if (userId == EvtRepository.Owner(evt))
                {
                    EvtRepository.Update(evt);
                    return evt.ID;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "UpdateEvent", Exception = ex };
            }
            return new Guid("99999999-9999-9999-9999-999999999999");
        }
        public IEvent FindEvent(Guid eventId)
        {
            try
            {
                if (eventId == new Guid("99999999-9999-9999-9999-999999999999") || eventId == new Guid())
                   throw new Exception(string.Format("Invalid eventId passed to the FindEvent method: {0} ", eventId.ToString()));

                var eventList = EvtRepository.Find(evt => evt.ID == eventId);

                foreach (IEvent evt in eventList)
                    return evt;
                return null;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "FindEvent", Exception = ex };
                return null;
            }
        }
        public void AddContactsToEvent(Guid userId, Guid eventId, List<Guid> contactList)
        {
            try
            {
                IEvent evt = FindEvent(eventId);

                var ownerId = EvtRepository.Owner(evt);
                if (userId == ownerId)
                    return;

                using (UserFacade userFacade = new UserFacade())
                {

                    var owner = userFacade.FindUser(ownerId);

                    foreach (var contactId in contactList)
                    {
                        if (!EvtRepository.Guest(evt, contactId))
                            continue;

                        IUser contact = userFacade.FindUser(userId);
                        if (contact != null)
                        {
                            EvtRepository.LinkToEvent(contact, evt);

                            //Here Add code for Real Time Framework
                            var invitesChannel = RTBroker.CreateUserChannel(contact);

                            var eventList = userFacade.RetrieveEventsByAttendanceStatus(contactId, EventAttendantsStatus.All);
                            var convertedEventList = RTBroker.ConvertEventToTableName(eventList);  //Do we have to redo all events or just the one added?
                            RTBroker.AuthUser(convertedEventList, contact.Token);
                            RTBroker.SendInvite(contact, evt.ID.ToString(), owner.Name);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "AddContactsToEvent", Exception = ex };
            }
        }
        public ResultType ChangeContactAttendanceStatus(Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            try
            {
                if (attendStatus == EventAttendantsStatus.All || attendStatus == EventAttendantsStatus.Cancel)
                    return ResultType.Error;

                IEvent evt = FindEvent(eventId);

                if (evt == null)
                    return ResultType.RecordNotFound; 

                if (!EvtRepository.Guest(evt, userId))
                    return ResultType.Error;

                EvtRepository.ChangeUserAttendStatus(userId, eventId, attendStatus);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "ChangeContactAttendanceStatus", Exception = ex };
                return ResultType.Error;
            }
            return ResultType.Successful;
        }
        public IEnumerable<IUser> InviteesByStatus(Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            try
            {
                if (userId == new Guid("99999999-9999-9999-9999-999999999999") || userId == new Guid())
                    throw new Exception(string.Format("Invalid UserId passed to the InviteesByStatus method: {0} ", userId.ToString()));

                if (eventId == new Guid("99999999-9999-9999-9999-999999999999") || eventId == new Guid())
                    throw new Exception(string.Format("Invalid eventId passed to the InviteesByStatus method: {0} ", eventId.ToString()));

                IEvent evt = FindEvent(eventId);
                if (!EvtRepository.Guest(evt, userId) && (userId != EvtRepository.Owner(evt)))
                    return null;

                var results = EvtRepository.AttendingByStatus(eventId, attendStatus);
                return results;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "EventAttendeesByStatus", Exception = ex };
                return null;
            }
        }
        public void Dispose()
        {
        }
        #endregion

        #region Private Methods

        private bool ValidateEventRequiredFields(IEvent evt, bool isNew)
        {
            bool passed = true;
            passed &= (evt != null);
            passed &= (evt.ID != new Guid("99999999-9999-9999-9999-999999999999"));
            passed &= (isNew ^ (evt.ID != new Guid("00000000-0000-0000-0000-000000000000")));
            return passed;
        }
        #endregion
    }
}
