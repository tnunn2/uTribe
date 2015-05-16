using System;
using NUnit.Framework;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Common;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.Test.RepositoryMocks;
using System.Collections.Generic;

namespace urTribeWebAPI.Test.BAL
{
    [TestFixture]
    class EventFacadeTest
    {
        [SetUp]
        public void Init()
        {
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.Evt = null;
            EventRepositoryMock<ScheduledEvent>.OwnerId = new Guid();
            EventRepositoryMock<ScheduledEvent>.EventId = new Guid();
            EventRepositoryMock<ScheduledEvent>.IsGuest = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = null;
            EventRepositoryMock<ScheduledEvent>.ListOfUsers = null;
            EventRepositoryMock<ScheduledEvent>.User = null;
            EventRepositoryMock<ScheduledEvent>.UserId = new Guid();
            EventRepositoryMock<ScheduledEvent>.AttendStatus = EventAttendantsStatus.Pending;
        }

        #region UpdateEvent
        [Test]
        public void UpdateEventWhenExceptionHappensReturnInvalidIdCode ()
        {
            EventRepositoryMock<ScheduledEvent>.ThrowException = true;
            Guid eventId = new Guid();
            Guid userId = Guid.NewGuid ();
            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true};
                    eventId = facade.UpdateEvent(userId, evt);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.AreEqual(eventId.ToString(), "99999999-9999-9999-9999-999999999999");
        }
        [Test]
        public void UpdateEventWithValidParametersReturnValidGuid ()
        {
            Guid mockEventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.EventId = mockEventId;
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;

            Guid eventId = new Guid ();

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    IEvent evt = new ScheduledEvent() { ID = mockEventId, Name = "Test Update", Active = true };
                    eventId = facade.UpdateEvent(userId, evt);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.AreEqual(eventId, EventRepositoryMock<ScheduledEvent>.EventId);
        }
        [Test]
        public void UpdateEventWithValidParametersCheckEventIsSentToRepository ()
        {
            Guid mockEventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.EventId = mockEventId;
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;
            EventRepositoryMock<ScheduledEvent>.Evt = null;

            IEvent evt = new ScheduledEvent() { ID = mockEventId, Name = "Test Update", Active = true };
            Guid eventId = new Guid();

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    eventId = facade.UpdateEvent(userId, evt);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.AreEqual(evt, EventRepositoryMock<ScheduledEvent>.Evt);
        }
        [Test]
        public void UpdateEventWithNULLEventObjectReturnInvalidIdCode()
        {
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.EventId = new Guid();
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;
            EventRepositoryMock<ScheduledEvent>.Evt = null;
            Guid eventId = new Guid();
            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    IEvent evt = null;
                    eventId = facade.UpdateEvent(userId, evt);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.AreEqual(eventId.ToString(), "99999999-9999-9999-9999-999999999999");
        }
        [Test]
        public void UpdateEventWithEventIdSetToZerosReturnInvalidIdCode ()
        {
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;
            Guid eventId = new Guid();


            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    IEvent evt = new ScheduledEvent() { ID = new Guid(), Name = "Test Update", Active = true };
                    eventId = facade.UpdateEvent(userId, evt);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.AreEqual(new Guid("99999999-9999-9999-9999-999999999999"), eventId);
        }
        [Test]
        public void UpdateEventWithEventIdSetToNinesReturnInvalidIdCode()
        {
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;
            Guid eventId = new Guid();


            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    IEvent evt = new ScheduledEvent() { ID = new Guid("99999999-9999-9999-9999-999999999999"), Name = "Test Update", Active = true };
                    eventId = facade.UpdateEvent(userId, evt);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.AreEqual(new Guid("99999999-9999-9999-9999-999999999999"), eventId);
        }
        #endregion

        #region FindEvent
        [Test]
        public void FindEventWhenExceptionHappensReturningNullEvent ()
        {

            IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true };
            List<IEvent> eventList = new List<IEvent>();
            eventList.Add(evt);

            EventRepositoryMock<ScheduledEvent>.ThrowException = true;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;
            IEvent result = new ScheduledEvent();


            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.FindEvent(evt.ID);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.AreEqual(null, result);
        }
        [Test]
        public void FindEventPassingEventIdAsZerosReturnNullEvent()
        {
            IEvent evt = new ScheduledEvent() { ID = new Guid(), Name = "Test Update", Active = true };
            List<IEvent> eventList = new List<IEvent>();
            eventList.Add(evt);

            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;
            IEvent result = new ScheduledEvent();


            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.FindEvent(EventRepositoryMock<ScheduledEvent>.EventId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {1}", ex.Message);
            }

            Assert.AreEqual(null, result);
        }
        [Test]
        public void FindEventPassingEventIdAsNinesReturnNullEvent ()
        {
            IEvent evt = new ScheduledEvent() { ID = new Guid("99999999-9999-9999-9999-999999999999"), Name = "Test Update", Active = true };
            List<IEvent> eventList = new List<IEvent>();
            eventList.Add(evt);

            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;
            IEvent result = new ScheduledEvent();


            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.FindEvent(EventRepositoryMock<ScheduledEvent>.EventId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {1}", ex.Message);
            }

            Assert.AreEqual(null, result);
        }
        [Test]
        public void FindEventPassingValidEventIdThatExistReturnEvent()
        {
            IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true };
            List<IEvent> eventList = new List<IEvent>();
            eventList.Add(evt);

            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;
            IEvent result = new ScheduledEvent();


            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.FindEvent(evt.ID);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.AreEqual(evt, result);
        }
        [Test]
        public void FindEventPassingValidEventIdThatNotExistReturnNullEvent()
        {
            List<IEvent> eventList = new List<IEvent>();

            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;
            IEvent result = new ScheduledEvent();


            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.FindEvent(Guid.NewGuid());
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.AreEqual(null, result);
        }
        #endregion

        #region AddContactsToEvent

        #endregion

        #region ChangeContactAttendanceStatus
        [Test]
        public void ChangeContactAttendanceStatusWithStatusOfAllReturnResultOfError()
        {
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ResultType result = ResultType.incomplete;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.All);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.AreEqual(ResultType.Error, result);
        }
        [Test]
        public void ChangeContactAttendanceStatusWithStatusOfCancelReturnResultOfError()
        {
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ResultType result = ResultType.incomplete;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Cancel);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.AreEqual(ResultType.Error, result);
        }
        [Test]
        public void ChangeContactAttendanceStatusWithStatusOfAllCheckIfSentToRepository()
        {
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ResultType result = ResultType.incomplete;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.All);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.AreNotEqual(EventRepositoryMock<ScheduledEvent>.AttendStatus, EventAttendantsStatus.All);
        }
        [Test]
        public void ChangeContactAttendanceStatusWithStatusOfCancelCheckIfSentToRepository()
        {
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ResultType result = ResultType.incomplete;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Cancel);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.AreNotEqual(EventRepositoryMock<ScheduledEvent>.AttendStatus, EventAttendantsStatus.All);
        }
        [Test]
        public void ChangeContactAttendanceStatusWithNoMatchingEventReturnResultOfRecordNotFound()
        {
            IEvent evt = null;
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ResultType result = ResultType.incomplete;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = false;
            EventRepositoryMock<ScheduledEvent>.Evt = evt;

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Attending);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.AreEqual(ResultType.RecordNotFound, result);
        }
        [Test]
        public void ChangeContactAttendanceStatusUserNotAGuestToEventReturnResultOfError()
        {
            IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true };
            IEnumerable<IEvent> eventList = new List<IEvent>() { evt };

            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ResultType result = ResultType.incomplete;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Attending);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.AreEqual(ResultType.Error, result);
        }
        [Test]
        public void ChangeContactAttendanceStatusWhenExceptionWithExceptionCaughtAndHandled()
        {
            IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true };
            IEnumerable<IEvent> eventList = new List<IEvent>() { evt };

            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ResultType result = ResultType.incomplete;
            EventRepositoryMock<ScheduledEvent>.ThrowException = true;
            EventRepositoryMock<ScheduledEvent>.IsGuest = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Attending);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.Pass();
        }
        [Test]
        public void ChangeContactAttendanceStatusValidUserAndValidEventReturnResultOfSucessful()
        {
            IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true };
            IEnumerable<IEvent> eventList = new List<IEvent>() { evt };

            Guid eventId = evt.ID;
            Guid userId = Guid.NewGuid();
            ResultType result = ResultType.incomplete;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;

            try
            {
                using (EventFacade facade = new EventFacade())
                {
                    result = facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Attending);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.AreEqual(ResultType.Successful, result);
        }
        #endregion

        #region InviteesByStatus
        #endregion
    }
}
