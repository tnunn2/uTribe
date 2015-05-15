using System;
using NUnit.Framework;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Common;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.Test.RepositoryMocks;

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
        }

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
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {1}", ex.Message);
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
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {1}", ex.Message);
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
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {1}", ex.Message);
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
                Assert.Fail("Unexpected exception escaped call to UpdateEvent method in the EventFacade. Exception: {1}", ex.Message);
            }

            Assert.AreEqual(eventId.ToString(), "99999999-9999-9999-9999-999999999999");
        }

    }
}
