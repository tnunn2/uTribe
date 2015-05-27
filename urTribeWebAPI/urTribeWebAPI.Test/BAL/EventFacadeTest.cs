using System;
using NUnit.Framework;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Common;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.Test.RepositoryMocks;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Newtonsoft.Json;
using urTribeWebAPI.Messaging;
using urTribeWebAPI.Messaging.RTFHelperClasses;

namespace urTribeWebAPI.Test.BAL
{
    [TestFixture]
    class EventFacadeTest

    {
        private static Mock<IMessageConnect> mockConnect;
        private static string creationURL = urTribeWebAPI.Messaging.Properties.Settings.Default.RTFCreateURL;
        private static Expression<Func<string, bool>> isCreation = url => url.Equals(creationURL);
        private static string authURL = urTribeWebAPI.Messaging.Properties.Settings.Default.RTFAuthURL;
        private static Func<string, bool> isAuth = (url => url.Equals(authURL));


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
            EventRepositoryMock<ScheduledEvent>.AttendStatus = EventAttendantsStatus.Invited;


            mockConnect = ConnectMockFactory.newMock();
        }

        #region UpdateEvent
        [Test]
        public void UpdateEventWhenExceptionHappensInRepositoryCausesExceptionToBubbleUp ()
        {
            EventRepositoryMock<ScheduledEvent>.ThrowException = true;
            Guid userId = Guid.NewGuid ();
            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true };
                    facade.UpdateEvent(userId, evt);
                }
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail("Error Message did not bubble up as expected");

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

            IEvent evt = new ScheduledEvent() { ID = mockEventId, Name = "Test Update", Time = "2015-05-24T17:02:39Z", Active = true };

            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                facade.UpdateEvent(userId, evt);

            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.AreEqual(evt, EventRepositoryMock<ScheduledEvent>.Evt);
        }
        [Test]
        public void UpdateEventWithNULLEventObjectReturnInvalidIEventObjectException()
        {
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.EventId = new Guid();
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;
            EventRepositoryMock<ScheduledEvent>.Evt = null;
            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    IEvent evt = null;
                    facade.UpdateEvent(userId, evt);
                }
            }
            catch (InvalidIEventObjectException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.Fail("No Exception was bubbled up.");
        }
        [Test]
        public void UpdateEventWithEventIdSetToZerosReturnInvalidIEventObjectException()
        {
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;


            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    IEvent evt = new ScheduledEvent() { ID = new Guid(), Name = "Test Update", Active = true };
                    facade.UpdateEvent(userId, evt);
                }
            }
            catch (InvalidIEventObjectException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.Fail("No Exception was bubbled up.");
        }
        [Test]
        public void UpdateEventWithEventIdSetToNinesReturnInvalidIEventObjectException()
        {
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;
            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    IEvent evt = new ScheduledEvent() { ID = new Guid("99999999-9999-9999-9999-999999999999"), Name = "Test Update", Active = true };
                    facade.UpdateEvent(userId, evt);
                }
            }
            catch (InvalidIEventObjectException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {0}", ex.Message);
            }

            Assert.Fail("No Exception was bubbled up.");
        }
        #endregion

        #region FindEvent
        [Test]
        public void FindEventWhenRepositoryExceptionHappensReturningNullEvent ()
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
                Assert.Fail("Exception from repository should be allow to bubble up");
            }
            catch (Exception)
            {
                Assert.Pass();
            }

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
                Assert.Fail("Expecting to receive an InvalidEventId Exception.");
            }
            catch (InvalidEventIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {1}", ex.Message);
            }

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
                Assert.Fail("Expecting to receive an InvalidEventId Exception.");
            }
            catch (InvalidEventIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {1}", ex.Message);
            }
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
        public void ChangeContactAttendanceStatusWithStatusOfAllReturnInvalidEventStatusException()
        {
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;

            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.All);
                }
            }
            catch (InvalidEventStatusException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.Fail("An Invalid Event Status exception should have been thrown.");
        }
        [Test]
        public void ChangeContactAttendanceStatusWithStatusOfCancelReturnInvalidEventStatusException()
        {
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;

            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Cancel);
                    Assert.Fail("An InvalidEventStatusException should have been thrown.");
                }
            }
            catch (InvalidEventStatusException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            
        }
        [Test]
        public void ChangeContactAttendanceStatusWithStatusOfAllCheckIfSentToRepository()
        {
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;

            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.All);
                }
            }
            catch (Exception)
            {
            }
            Assert.AreNotEqual(EventRepositoryMock<ScheduledEvent>.AttendStatus, EventAttendantsStatus.All);
        }
        [Test]
        public void ChangeContactAttendanceStatusWithStatusOfCancelCheckIfSentToRepository()
        {
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;

            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Cancel);
                }
            }
            catch (Exception)
            {
            }
            Assert.AreNotEqual(EventRepositoryMock<ScheduledEvent>.AttendStatus, EventAttendantsStatus.Cancel);
        }
        [Test]
        public void ChangeContactAttendanceStatusWithNoMatchingEventReturnResultOfRecordNotFound()
        {
            IEvent evt = null;
            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = new List<IEvent>();
            EventRepositoryMock<ScheduledEvent>.Evt = evt;

            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Going);
                    Assert.Fail("An EventException should have been thrown.");
                }
            }
            catch (EventException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
        }
        [Test]
        public void ChangeContactAttendanceStatusUserNotAGuestToEventReturnResultOfError()
        {
            IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true };
            IEnumerable<IEvent> eventList = new List<IEvent>() { evt };

            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;

            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Going);
                    Assert.Fail("An EventException should have been thrown.");
                }
            }
            catch (EventException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
        }
        [Test]
        public void ChangeContactAttendanceStatusWhenExceptionWithExceptionCaughtAndHandled()
        {
            IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true };
            IEnumerable<IEvent> eventList = new List<IEvent>() { evt };

            Guid eventId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = true;
            EventRepositoryMock<ScheduledEvent>.IsGuest = false;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;

            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Going);
                }
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail("An exception from the Repository should have been thrown.");
        }
        [Test]
        public void ChangeContactAttendanceStatusValidUserAndValidEventReturnResultOfSucessful()
        {
            IEvent evt = new ScheduledEvent() { ID = Guid.NewGuid(), Name = "Test Update", Active = true };
            IEnumerable<IEvent> eventList = new List<IEvent>() { evt };

            Guid eventId = evt.ID;
            Guid userId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.IsGuest = true;
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = eventList;

            try
            {
                using (EventFacade facade = new EventFacade(mockConnect.Object))
                {
                    facade.ChangeContactAttendanceStatus(userId, eventId, EventAttendantsStatus.Going);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to FindEvent method in the EventFacade. Exception: {0}", ex.Message);
            }
            Assert.Pass();
        }
        #endregion

        #region InviteesByStatus
        #endregion
    }
}
