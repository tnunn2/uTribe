using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Policy;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Common;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.Messaging;
using urTribeWebAPI.Messaging.RTFHelperClasses;
using urTribeWebAPI.Test.RepositoryMocks;

namespace urTribeWebAPI.Test.BAL
{
    [TestFixture]
    class UserFacadeTest
    {
        private static Mock<IMessageConnect> mockConnect;


        [SetUp]
        public void Init()
        {
            UserRepositoryMock<User>.User = null;
            UserRepositoryMock<User>.UserId = new Guid();
            UserRepositoryMock<User>.FriendId = new Guid();
            UserRepositoryMock<User>.ListOfUsers = null;
            UserRepositoryMock<User>.ThrowException = false;

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

        #region CreateUser
        [Test]
        public void CreateUserWithEmptyFieldsReturnUserException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    IUser user = new User();
                    facade.CreateUser(user);
                }
            }
            catch (UserException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {0}", ex.Message);
            }

            Assert.Fail("An UserException should have been thrown because user not have passed validation");
        }
        [Test]
        public void CreateUserWithNullObjectReturnUserException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    IUser user = null;
                    facade.CreateUser(user);
                }
            }
            catch (UserException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {0}", ex.Message);
            }
        }
        [Test]
        public void CreateUserWithRequiredFieldsAndIDReturnUserException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    IUser user = new User() { Name = "Kevin Arnold", ID =  Guid.NewGuid() };
                    facade.CreateUser(user);
                }
            }
            catch (UserException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {0}", ex.Message);
            }
        }
        [Test]
        public void CreateUserWithRequiredFieldsAndInvalidIDReturnUserException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    IUser user = new User() { Name = "Kevin Arnold", ID = new Guid("99999999-9999-9999-9999-999999999999") };
                    facade.CreateUser(user);
                }
            }
            catch (UserException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {0}", ex.Message);
            }
            Assert.Fail("An UserException should have been generated");
        }
        [Test]
        public void CreateUserWhenExceptionHappensReturnInvalidIdCode()
        {
            UserRepositoryMock<User>.ThrowException = true;
            Guid userId = new Guid();
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    IUser user = new User() { Name = "Kevin Arnold" };
                    userId = facade.CreateUser(user);
                }
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail("An Exception should have been thrown from the repository");
        }
        [Test]
        public void CreateUserWithRequiredFieldReturnsUserId()
        {
            
            using (UserFacade facade = new UserFacade(mockConnect.Object))
            {
                IUser user = new User() { Name = "Kevin Arnold" };
                Guid userId = facade.CreateUser(user);

    
                Assert.IsTrue(userId != null && userId.ToString() != "99999999-9999-9999-9999-999999999999");
            }
        }
        #endregion

        #region UpdateUser
        [Test]
        public void UpdateUserWithNullUserObjectReturnUserException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    IUser user = null;
                    facade.UpdateUser(user);
                }
            }
            catch (UserException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call update on a user. Exception: {0}", ex.Message);
            }
            Assert.Fail("An UserException should have been generated");
        }
        [Test]
        public void UpdateUserWithZerosAsUserIdReturnUserException ()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    IUser user = new User() { Name = "Kevin Arnold", ID = new Guid() };
                    facade.UpdateUser(user);
                }
            }
            catch (UserException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to update a user. Exception: {0}", ex.Message);
            }
            Assert.Fail("An UserException should have been generated");
        }
        [Test]
        public void UpdateUserWithNinesAsUserIdReturnUserException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    IUser user = new User() { Name = "Kevin Arnold", ID = new Guid("99999999-9999-9999-9999-999999999999") };
                    facade.UpdateUser(user);
                }
            }
            catch (UserException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to update a user. Exception: {0}", ex.Message);
            }
            Assert.Fail("An UserException should have been generated");
        }
        [Test]
        public void UpdateUserWhenExceptionOccursInRepositoryReturnUserException()
        {
            UserRepositoryMock<User>.ThrowException = true;
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    IUser user = new User() { Name = "Kevin Arnold", ID = Guid.NewGuid () };
                    facade.UpdateUser(user);
                }
            }
            catch (Exception)
            {
                Assert.Pass();
            }
                Assert.Fail("An Exception should have been generated");
        }
        [Test]
        public void UpdateUserWithNinesAsUserIdSendingUserToRepository()
        {
            UserRepositoryMock<User>.User = null;
            IUser user = null;
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    user = new User() { Name = "Kevin Arnold", ID = Guid.NewGuid() };
                    facade.UpdateUser(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to update a user. Exception: {0}", ex.Message);
            }
            Assert.IsTrue(user == UserRepositoryMock<User>.User);
        }
        #endregion

        #region FindUser
        [Test]
        public void FindUserWithValidIdReturnUser()
        {
            var usrId = Guid.NewGuid();
            IUser user = new User();

            List<User> ListOfUsers = new List<User>();
            ListOfUsers.Add(new User() { Name = "Jack Frost", ID = usrId });
            UserRepositoryMock<User>.ListOfUsers = ListOfUsers;

            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    user = facade.FindUser(usrId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to find a user. Exception: {0}", ex.Message);
            }
            Assert.IsTrue(user.Name == "Jack Frost");
        }
        [Test]
        public void FindUserWithZerosAsIdReturnInvalidUserIdException()
        {
            var usrId = new Guid();
            IUser user = new User();

            List<User> ListOfUsers = new List<User>();
            ListOfUsers.Add(new User() { Name = "Jack Frost", ID = usrId });
            UserRepositoryMock<User>.ListOfUsers = ListOfUsers;

            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    user = facade.FindUser(usrId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to find a user. Exception: {0}", ex.Message);
            }
            Assert.Fail("An InvalidUserIdException should have been thrown for the invliad Id.");
        }
        [Test]
        public void FindUserWithNinesAsIdReturnInvalidUserIdException()
        {
            var usrId = new Guid("99999999-9999-9999-9999-999999999999");
            IUser user = new User();

            List<User> ListOfUsers = new List<User>();
            ListOfUsers.Add(new User() { Name = "Jack Frost", ID = usrId });
            UserRepositoryMock<User>.ListOfUsers = ListOfUsers;

            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    user = facade.FindUser(usrId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to find a user. Exception: {0}", ex.Message);
            }
            Assert.Fail("An InvalidUserIdException should have been thrown for the invliad Id.");
        }
        [Test]
        public void FindUserUsingNonExistingUserIdReturnNull()
        {
            var usrId = Guid.NewGuid();
            IUser user = new User();
            UserRepositoryMock<User>.ListOfUsers = new List<IUser>();

            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    user = facade.FindUser(usrId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to find a user. Exception: {0}", ex.Message);
            }
            Assert.IsTrue(user == null);
        }
        [Test]
        public void FindUserWhenExceptionHappensReturnNull()
        {
            var usrId = Guid.NewGuid();
            IUser user = null;
            UserRepositoryMock<User>.ThrowException = true;

            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    user = facade.FindUser(usrId);
                }
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail("An Exception from the repository should have bubbled up.");
        }
        #endregion

        #region AddContact
        [Test]
        public void AddContactWithUserIdAsZerosReturnInvalidUserIdException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    Guid userId = new Guid();
                    Guid friendId = Guid.NewGuid();

                    facade.AddContact(userId, friendId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to Link Contact to User. Exception: {0}", ex.Message);
            }
            Assert.Fail("An UserException should have been generated");
        }
        [Test]
        public void AddContactWithUserIdAsNinesReturnInvalidUserIdException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    Guid userId = new Guid("99999999-9999-9999-9999-999999999999");
                    Guid friendId = Guid.NewGuid();

                    facade.AddContact(userId, friendId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to Link Contact to User. Exception: {0}", ex.Message);
            }
            Assert.Fail("An UserException should have been generated");
        }
        [Test]
        public void AddContactWithFriendIdAsZerosReturnInvalidUserIdException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    Guid userId = Guid.NewGuid();
                    Guid friendId = new Guid();

                    facade.AddContact(userId, friendId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to Link Contact to User. Exception: {0}", ex.Message);
            }
            Assert.Fail("An UserException should have been generated");
        }
        [Test]
        public void AddContactWithFriendIdAsNinesReturnInvalidUserIdException()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    Guid userId = Guid.NewGuid();
                    Guid friendId = new Guid("99999999-9999-9999-9999-999999999999");

                    facade.AddContact(userId, friendId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to Link Contact to User. Exception: {0}", ex.Message);
            }
            Assert.Fail("An UserException should have been generated");
        }
        [Test]
        public void AddContactWithValidUserAndFriendIDsReturnNothing ()
        {
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    Guid userId = Guid.NewGuid();
                    Guid friendId = Guid.NewGuid();

                    facade.AddContact(userId, friendId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {0}", ex.Message);
            }

            Assert.Pass();
        }
        [Test]
        public void AddContactWhenExceptionHappensReturnNothing()
        {
            UserRepositoryMock<User>.ThrowException = true;
            try
            {
                using (UserFacade facade = new UserFacade(mockConnect.Object))
                {
                    Guid userId = Guid.NewGuid();
                    Guid friendId = Guid.NewGuid();

                    facade.AddContact(userId, friendId);
                }
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail("An Exception should have bubbled up from the Repository.");
        }
        #endregion

        #region RetrieveContacts
        [Test]
        public void RetrieveContactWithUserIdAsZerosReturnInvalidUserException()
        {
            var usrId = new Guid();
            IEnumerable<IUser> userfriends = null;
            List<IUser> friends = new List<IUser>();
            friends.Add(new User() { Name = "Jack Frost", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Tom Ridder", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Uni Daisy", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Lisa Hunter", ID = Guid.NewGuid() });
            UserRepositoryMock<User>.ListOfUsers = friends;

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    userfriends = facade.RetrieveContacts(usrId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to retrieve contacts. Exception: {0}", ex.Message);
            }

            Assert.Fail("An InvalidUserIdException should have been thrown");
        }
        [Test]
        public void RetrieveContactWithUserIdAsNinesReturnInvalidUserException()
        {
            var usrId = new Guid("99999999-9999-9999-9999-999999999999");
            IEnumerable<IUser> userfriends = null;
            List<IUser> friends = new List<IUser>();
            friends.Add(new User() { Name = "Jack Frost", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Tom Ridder", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Uni Daisy", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Lisa Hunter", ID = Guid.NewGuid() });
            UserRepositoryMock<User>.ListOfUsers = friends;

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    userfriends = facade.RetrieveContacts(usrId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to retrieve contacts. Exception: {0}", ex.Message);
            }

            Assert.Fail("An InvalidUserIdException should have been thrown");
        }
        [Test]
        public void RetrieveContactsValidIdReturnCorrectList ()
        {
            var usrId = Guid.NewGuid();
            IEnumerable<IUser> userfriends = null;
            List<IUser> friends = new List<IUser>();
            friends.Add(new User() { Name = "Jack Frost", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Tom Ridder", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Uni Daisy", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Lisa Hunter", ID = Guid.NewGuid() });
            UserRepositoryMock<User>.ListOfUsers = friends;

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    userfriends = facade.RetrieveContacts(usrId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to retrieve contacts. Exception: {0}", ex.Message);
            }

            Assert.IsTrue(userfriends == friends);
        }
        [Test]
        public void RetrieveContactsRepositoryExceptionHappensReturnException()
        {
            var usrId = Guid.NewGuid();
            IEnumerable<IUser> userfriends = null;
            List<IUser> friends = new List<IUser>();
            friends.Add(new User() { Name = "Jack Frost", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Tom Ridder", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Uni Daisy", ID = Guid.NewGuid() });
            friends.Add(new User() { Name = "Lisa Hunter", ID = Guid.NewGuid() });
            UserRepositoryMock<User>.ListOfUsers = friends;

            UserRepositoryMock<User>.ThrowException = true;

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    userfriends = facade.RetrieveContacts(usrId);
                }
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail("An exception should have bubbled up from the repository");
        }
        #endregion

        #region RetrieveEventsByAttendanceStatus
        #endregion

        #region RemoveContact
        [Test]
        public void RemoveContactWithValidUserAndFriendIDsReturnNothing()
        {
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    Guid userId = Guid.NewGuid();
                    Guid friendId = Guid.NewGuid();

                    facade.RemoveContact(userId, friendId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {0}", ex.Message);
            }

            Assert.Pass();
        }
        [Test]
        public void RemoveContactWhenRepositoryExceptionHappensReturnException()
        {
            UserRepositoryMock<User>.ThrowException = true;
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    Guid userId = Guid.NewGuid();
                    Guid friendId = Guid.NewGuid();

                    facade.RemoveContact(userId, friendId);
                }
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail("An Exception from the repository should have bubbled up.");
        }
        #endregion

        #region CreateEvent
        #endregion

        #region CancelEvent
        [Test]
        public void CancelEventUserIdAsZerosReturnInvalidUserIdException()
        {
            Guid userId = new Guid();
            Guid eventId = Guid.NewGuid();

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.CancelEvent(userId, eventId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception happened while testing CancelEvent. Exception: {0}", ex.Message);
            }

            Assert.Fail("An InvalidUserIdException should have been thrown");
        }
        [Test]
        public void CancelEventUserIdAsNinesReturnInvalidUserIdException()
        {
            Guid userId = new Guid("99999999-9999-9999-9999-999999999999");
            Guid eventId = Guid.NewGuid();

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.CancelEvent(userId, eventId);
                }
            }
            catch (InvalidUserIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception happened while testing CancelEvent. Exception: {0}", ex.Message);
            }

            Assert.Fail("An InvalidUserIdException should have been thrown");
        }
        [Test]
        public void CancelEventEventIdAsZerosReturnInvalidEventIdException()
        {
            Guid userId = Guid.NewGuid();
            Guid eventId = new Guid();

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.CancelEvent(userId, eventId);
                }
            }
            catch (InvalidEventIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception happened while testing CancelEvent. Exception: {0}", ex.Message);
            }

            Assert.Fail("An InvalidEventIdException should have been thrown");
        }
        [Test]
        public void CancelEventEventIdAsNinesReturnInvalidEventIdException()
        {
            Guid userId = Guid.NewGuid();
            Guid eventId = new Guid("99999999-9999-9999-9999-999999999999");

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.CancelEvent(userId, eventId);
                }
            }
            catch (InvalidEventIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception happened while testing CancelEvent. Exception: {0}", ex.Message);
            }

            Assert.Fail("An InvalidEventIdException should have been thrown");
        }
        [Test]
        public void CancelEventNoMatchingRecordsReturnInvalidEventIdException()
        {
            Guid userId = Guid.NewGuid();
            Guid eventId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = new List<IEvent>();
            UserRepositoryMock<User>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.CancelEvent(userId, eventId);
                }
            }
            catch (InvalidEventIdException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception happened while testing CancelEvent. Exception: {0}", ex.Message);
            }

            Assert.Fail("An InvalidEventIdException should have been thrown");
        }
        [Test]
        public void CancelEventUserNoOwnerReturnEventException()
        {
            Guid userId = Guid.NewGuid();
            Guid eventId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.OwnerId = Guid.NewGuid();


            var evtList = new List<IEvent>();
            evtList.Add(new ScheduledEvent() { ID = eventId, Name = "Test Event", Active = true });
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = evtList;


            UserRepositoryMock<User>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.CancelEvent(userId, eventId);
                }
            }
            catch (EventException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception happened while testing CancelEvent. Exception: {0}", ex.Message);
            }

            Assert.Fail("An InvalidEventIdException should have been thrown");
        }
        [Test]
        public void CancelEventEventNotActiveReturnEventException()
        {
            Guid userId = Guid.NewGuid();
            Guid eventId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;


            var evtList = new List<IEvent>();
            evtList.Add(new ScheduledEvent() { ID = eventId, Name = "Test Event", Active = false });
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = evtList;


            UserRepositoryMock<User>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.CancelEvent(userId, eventId);
                }
            }
            catch (EventException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception happened while testing CancelEvent. Exception: {0}", ex.Message);
            }

            Assert.Fail("An InvalidEventIdException should have been thrown");
        }
        [Test]
        public void CancelEventSuccessfulReturnNothing()
        {
            Guid userId = Guid.NewGuid();
            Guid eventId = Guid.NewGuid();
            EventRepositoryMock<ScheduledEvent>.OwnerId = userId;


            var evtList = new List<IEvent>();
            evtList.Add(new ScheduledEvent() { ID = eventId, Name = "Test Event", Time = "2015-05-24T17:02:39Z", Active = true });
            EventRepositoryMock<ScheduledEvent>.ListOfEvents = evtList;


            UserRepositoryMock<User>.ThrowException = false;
            EventRepositoryMock<ScheduledEvent>.ThrowException = false;
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    facade.CancelEvent(userId, eventId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception happened while testing CancelEvent. Exception: {0}", ex.Message);
            }

            Assert.Pass();
        }
        #endregion

    }
}
