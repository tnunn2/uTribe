using System;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.Common.Concrete;
using urTribeWebAPI.Common.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;

namespace urTribeWebAPI.Test.DAL
{
    [TestFixture]
    class FactoryTest
    {
        [Test]
        public void RetrieveRepository()
        {
//            var factory = RepositoryFactory.Instance;
//            IUserRepository userRepository = factory.Create<IUserRepository>();
//            Assert.IsTrue(userRepository != null);
        }

        [Test]
        public void RepositoryAddUserNode()
        {
//            var factory = RepositoryFactory.Instance;
//            IUserRepository userRepository = factory.Create<IUserRepository>();

//            User user = new User { ID = Guid.NewGuid()};
//            userRepository.Add(user);
//            Assert.IsTrue(userRepository != null);
        }

        [Test]
        public void RepositoryAddEventNode()
        {
//            var factory = RepositoryFactory.Instance;
//            IEventRepository eventRepository = factory.Create<IEventRepository>();
//            IUserRepository userRepository = factory.Create<IUserRepository>();
         
//            User user = new User { ID = Guid.NewGuid() };
//            userRepository.Add(user);

//            ScheduledEvent evt = new ScheduledEvent { ID = Guid.NewGuid() };
//            eventRepository.Add(user, evt);
//            Assert.IsTrue(evt != null);
        }

        [Test]
        public void RepositoryMergeUserNode()
        {
//            var factory = RepositoryFactory.Instance;
//           IUserRepository userRepository = factory.Create<IUserRepository>();

//            User user = new User { ID = new Guid("ec8cfb7f-4436-4f72-b0fc-62b9f134ff28") };
//            userRepository.Add(user);
//            Assert.IsTrue(userRepository != null);
        }

        [Test]
        public void RepositoryRemoveUserNode()
        {
//            var factory = RepositoryFactory.Instance;
//            IUserRepository userRepository = factory.Create<IUserRepository>();

//            User user = new User { ID = new Guid("ec8cfb7f-4436-4f72-b0fc-62b9f134ff28") };
//            userRepository.Remove(user);
//            Assert.IsTrue(userRepository != null);
        }

        [Test]
        public void RepositoryFindUserNode()
        {
//            var factory = RepositoryFactory.Instance;
//            IUserRepository userRepository = factory.Create<IUserRepository>();

//            var UserList = userRepository.Find(user => user.ID.ToString() == "ec8cfb7f-4436-4f72-b0fc-62b9f134ff28");
      //      Assert.IsTrue(UserList.Count > 0);
        }
        [Test]
        public void RepositoryAddToContactList()
        {
//            var factory = RepositoryFactory.Instance;
//            IUserRepository userRepository = factory.Create<IUserRepository>();

//            User user = new User { ID = Guid.NewGuid() };
//            userRepository.Add(user);
//            Assert.IsTrue(userRepository != null);

//            User friend = new User { ID = Guid.NewGuid() };
//            userRepository.Add(friend);
//            Assert.IsTrue(userRepository != null);

//            userRepository.AddToContactList(user, friend);
        }
    }
}
