using NUnit.Framework;
using System;
using urTribeWebAPI.Common;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.Test.RepositoryMocks;

namespace urTribeWebAPI.Test.DAL
{
    [TestFixture]
    class FactoryTest
    {
        [SetUp]
        public void Init()
        {
            UserRepositoryMock<User>.User = null;
            UserRepositoryMock<User>.UserId = new Guid();
            UserRepositoryMock<User>.FriendId = new Guid();
            UserRepositoryMock<User>.ListOfUsers = null;
            UserRepositoryMock<User>.ThrowException = false;
        }
        
        [Test]
        public void RetrieveUserRepositoryReturnUserRepositoryMock()
        {
            var factory = RepositoryFactory.Instance;
            IUserRepository userRepository = factory.Create<IUserRepository>();
            try
            {
                var userRepositoryMock = (UserRepositoryMock<User>)userRepository;
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected repository was returned instead of userRepositoryMock. Exception: {1}", ex.Message);
            }

            Assert.Pass();
        }

        [Test]
        public void RequestNonExistRepositoryReturnException()
        {
            var factory = RepositoryFactory.Instance;

            try
            {
                IFakeRepository fakeRepository = factory.Create<IFakeRepository>();
            }
            catch  (RepositoryNotExistException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected Exception returned. Exception: {1}", ex.Message);
            }

            Assert.Fail("RepositionNotExistException was expected.");
        }

    }
}
