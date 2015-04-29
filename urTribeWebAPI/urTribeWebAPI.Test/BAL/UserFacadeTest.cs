using System;
using System.Collections.Generic;
using NUnit.Framework;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Common.Concrete;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.Test.RepositoryMocks;

namespace urTribeWebAPI.Test.BAL
{
    [TestFixture]
    class UserFacadeTest
    {
        [SetUp]
        public void Init()
        {
            UserRepositoryMock<User>.User = null;
            UserRepositoryMock<User>.UsrId = new Guid();
            UserRepositoryMock<User>.FriendId = new Guid();
            UserRepositoryMock<User>.ListOfUsers = null;
            UserRepositoryMock<User>.ThrowException = false;
        }
        [Test]
        public void AddUserWithEmptyFieldsReturnInvalidIds ()
        {
            Guid userId = new Guid("99999999-9999-9999-9999-999999999999");

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    IUser user = new User();
                    userId = facade.CreateUser(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.AreEqual(userId.ToString(), "99999999-9999-9999-9999-999999999999");
        }
        [Test]
        public void AddUserWithNullObjectReturnInvalidIds()
        {
            Guid userId = new Guid("99999999-9999-9999-9999-999999999999"); ;

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    IUser user = null;
                    userId = facade.CreateUser(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.AreEqual(userId.ToString(), "99999999-9999-9999-9999-999999999999");
        }
        [Test]
        public void AddUserWithRequiredFieldsAndIDReturnInvalidIds()
        {
            Guid userId = new Guid("99999999-9999-9999-9999-999999999999"); ;

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    IUser user = new User() { Name = "Kevin Arnold", ID =  Guid.NewGuid() };
                    userId = facade.CreateUser(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.AreEqual(userId.ToString(), "99999999-9999-9999-9999-999999999999");
        }
        [Test]
        public void AddUserWithRequiredFieldsAndInvalidIDReturnInvalidIds()
        {
            Guid userId = new Guid("99999999-9999-9999-9999-999999999999");

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    IUser user = new User() { Name = "Kevin Arnold", ID = new Guid("99999999-9999-9999-9999-999999999999") };
                    userId = facade.CreateUser(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.AreEqual(userId.ToString(), "99999999-9999-9999-9999-999999999999");
        }
        [Test]
        public void AddUserWhenExceptionHappensReturnInvalidIdCode()
        {
            UserRepositoryMock<User>.ThrowException = true;
            Guid userId = new Guid();
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    IUser user = new User() { Name = "Kevin Arnold" };
                    userId = facade.CreateUser(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.AreEqual(userId.ToString(), "99999999-9999-9999-9999-999999999999");
        }
        [Test]
        public void AddUserWithRequiredFieldReturnsUserId()
        {
            using (UserFacade facade = new UserFacade())
            {
                IUser user = new User() { Name = "Kevin Arnold" };
                Guid userId = facade.CreateUser(user);

    
                Assert.IsTrue(userId != null && userId.ToString() != "99999999-9999-9999-9999-999999999999");
            }
        }
        [Test]
        public void FindUserWithValidIdReturnUser()
        {
            var usrId = Guid.NewGuid();

            List<User> ListOfUsers = new List<User> ();
            ListOfUsers.Add(new User() { Name = "Jack Frost",ID = usrId});
            UserRepositoryMock<User>.ListOfUsers = ListOfUsers;

            using (UserFacade facade = new UserFacade())
            {
                 var user = facade.FindUser(usrId);
                 Assert.IsTrue(user.Name == "Jack Frost");
            }
        }
        [Test]
        public void FindUserUsingInvalidIDReturnNull()
        {
            var usrId = Guid.NewGuid();

            using (UserFacade facade = new UserFacade())
            {
                var user = facade.FindUser(usrId);
                Assert.IsTrue(user == null);
            }
        }
        [Test]
        public void FindUserWhenExceptionHappensReturnNull()
        {
            var usrId = Guid.NewGuid();
            IUser user = null;
            UserRepositoryMock<User>.ThrowException = true;

            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    user = facade.FindUser(usrId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }
            Assert.IsTrue(user == null);
        }
        [Test]
        public void AddContactWithValidUserAndFriendIDsReturnNothing ()
        {
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    Guid userId = Guid.NewGuid();
                    Guid friendId = Guid.NewGuid();

                    facade.AddContact(userId, friendId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.Pass();
        }
        [Test]
        public void AddContactWhenExceptionHappensReturnNothing()
        {
            UserRepositoryMock<User>.ThrowException = true;
            try
            {
                using (UserFacade facade = new UserFacade())
                {
                    Guid userId = Guid.NewGuid();
                    Guid friendId = Guid.NewGuid();

                    facade.AddContact(userId, friendId);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.Pass();
        }
        [Test]
        public void RetrieveContactValidIdReturnCorrectList ()
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
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.IsTrue(userfriends == friends);
        }
        [Test]
        public void RetrieveContactExceptionHappensReturnNull()
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
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.IsTrue(userfriends == null);
        }
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
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.Pass();
        }
        [Test]
        public void RemoveContactWhenExceptionHappensReturnNothing()
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
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception escaped call to create user. Exception: {1}", ex.Message);
            }

            Assert.Pass();
        }

    }
}
