using System;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.Common.Concrete;

namespace urTribeWebAPI.DAL.DALTest
{
    class program
    {
        private User _testUser;
        private User _contactUser;
        private ScheduledEvent _event;


        private void TestAddUser ()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IUserRepository userRepository = factory.Create<IUserRepository>();

                User user = new User { Name = "Test User", ID = Guid.NewGuid() };
                userRepository.Add(user);
                _testUser = user;
                Console.WriteLine("Adding User Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Adding User Failed -Exception: {0}", ex.Message);
            }
        }
        private void TestUpdate ()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IUserRepository userRepository = factory.Create<IUserRepository>();

                _testUser.Name = "Update Test";
        
                userRepository.Update (_testUser);
                Console.WriteLine("Updating User Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Updating User Failed -Exception: {0}", ex.Message);
            }
        }
        private void TestFind ()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IUserRepository userRepository = factory.Create<IUserRepository>();

                var results = userRepository.Find(user => user.ID == _testUser.ID);

                Guid RID = new Guid();

                foreach (var result in results)
                    RID = result.ID;

                if (RID == _testUser.ID)
                   Console.WriteLine("Find User Passed");
                else
                    Console.WriteLine("Find User Failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Find User Failed -Exception: {0}", ex.Message);
            }
        }
        private void TestAddingContact ()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IUserRepository userRepository = factory.Create<IUserRepository>();

                User contact = new User { Name = "Test Contact", ID = Guid.NewGuid() };
                userRepository.Add(contact);
                _contactUser = contact;

                userRepository.AddToContactList(_testUser.ID, _contactUser.ID);

                Console.WriteLine("Adding Contacts Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Adding Contacts Failed -Exception: {0}", ex.Message);
            }
        }
        private void TestRetrieveContact ()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IUserRepository userRepository = factory.Create<IUserRepository>();

                var results = userRepository.RetrieveContacts (_testUser.ID);

                int cnt = 0;

                foreach (var result in results)
                    ++cnt;
 
                if (cnt > 0)
                   Console.WriteLine("Retrieving Contacts Passed");
                else
                    Console.WriteLine("Retrieving Contacts Failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Retrieving Contacts Failed -Exception: {0}", ex.Message);
            }
        }
        private void TestAddingEvent()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IEventRepository eventRepository = factory.Create<IEventRepository>();

                _event = new ScheduledEvent () { ID = Guid.NewGuid()};
                eventRepository.Add(_testUser, _event);

                Console.WriteLine("Adding Event Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Adding Event Failed -Exception: {0}", ex.Message);
            }

        }
        private void TestLinkingToEvent ()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IEventRepository eventRepository = factory.Create<IEventRepository>();

                eventRepository.LinkToEvent(_contactUser, _event);
                Console.WriteLine("Link Event Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Linking Event Failed -Exception: {0}", ex.Message);
            }

        }
        private void TestUpdateEvent()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IEventRepository eventRepository = factory.Create<IEventRepository>();

                _event.VenueName = "Update Event";

                eventRepository.Update(_event);

                Console.WriteLine("Update Event Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update Event Failed -Exception: {0}", ex.Message);
            }

        }
        private void TestFindEvent ()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IEventRepository eventRepository = factory.Create<IEventRepository>();

                var results = eventRepository.Find(evt => evt.ID == _event.ID);

                Guid RID = new Guid();

                foreach (var result in results)
                    RID = result.ID;

                if (RID == _event.ID)
                   Console.WriteLine("Find Event Passed");
                else
                    Console.WriteLine("Find Event Failed");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Find Event Failed -Exception: {0}", ex.Message);
            }

        }
        private void TestChangeUserAttendStatus()
        {
            try
            {
                var factory = RepositoryFactory.Instance;
                IEventRepository eventRepository = factory.Create<IEventRepository>();

                eventRepository.ChangeUserAttendStatus(_contactUser.ID, _event.ID, Common.EventAttendantsStatus.Cancel);
                Console.WriteLine("ChangeUserAttendStatus Event Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ChangeUserAttendStatus Failed -Exception: {0}", ex.Message);
            }
        }

        private void RunNeo4jTest ()
        {
            TestAddUser();
            TestUpdate();
            TestFind();
            TestAddingContact();
            TestRetrieveContact();

            TestAddingEvent();
            TestUpdateEvent();
            TestLinkingToEvent();
            TestFindEvent();
            TestChangeUserAttendStatus();
        }

        static void Main(string[] args)
        {
            program p = new program();
            p.RunNeo4jTest();
            Console.ReadLine();
        }
    }

}
