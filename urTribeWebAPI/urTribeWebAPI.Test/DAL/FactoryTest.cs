using System;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.Common.Concrete;
using NUnit.Framework;

namespace urTribeWebAPI.Test.DAL
{
    [TestFixture]
    class FactoryTest
    {
        [Test]
        public void RetrieveRepository()
        {
            //var factory = RepositoryFactory.Instance;
            //IUserRepository userRepository = factory.Create<IUserRepository>();
            ////Assert.IsTrue(userRepository != null);
        }

        [Test]
        public void RepositoryAddUserNode()
        {
            //var factory = RepositoryFactory.Instance;
            //IUserRepository userRepository = factory.Create<IUserRepository>();

            //User user = new User { ID = Guid.NewGuid()};
            //userRepository.Add(user);
            //Assert.IsTrue(userRepository != null);
        }

        [Test]
        public void RepositoryMergeUserNode()
        {
            //var factory = RepositoryFactory.Instance;
            //IUserRepository userRepository = factory.Create<IUserRepository>();

            //User user = new User { ID = new Guid("58770a88-ba50-4d1e-a6d8-d98a252427f1") };
            //userRepository.Add(user);
            //Assert.IsTrue(userRepository != null);

        }

    }
}
