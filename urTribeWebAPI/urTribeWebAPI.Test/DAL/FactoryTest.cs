using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urTribeWebAPI.Common;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.DAL.Repositories;
using NUnit.Framework;
using Moq;

namespace urTribeWebAPI.Test.DAL
{
    [TestFixture]
    class FactoryTest
    {
        [Test]
        public void RetrieveRepository()
        {
            var factory = RepositoryFactory.Instance;
            IUserRepository userRepository = factory.Create<IUserRepository>();
            Assert.IsTrue(userRepository != null);
        }           
    }
}
