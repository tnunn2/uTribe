using System;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.Common.Concrete;

namespace urTribeWebAPI.DAL.DALTest
{
    class program
    {
        private void TestAddUser ()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IUserRepository userRepository = factory.Create<IUserRepository>();

                User user = new User { Name = "Test User", ID = Guid.NewGuid() };
                userRepository.Add(user);
                Console.WriteLine("Adding User Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Adding User Failed -Exception: {0}", ex.Message);
            }
        }

        private void RunNeo4jTest ()
        {
            TestAddUser();
        }
        static void Main(string[] args)
        {
            program p = new program();
            p.RunNeo4jTest();
            Console.ReadLine();
        }
    }

}
