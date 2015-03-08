using System;
using System.Collections.Generic;
using Neo4jClient;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Common.Concrete;
using urTribeWebAPI.DAL.Interfaces;
using System.Configuration;

namespace urTribeWebAPI.DAL.Repositories
{
    public class UserRepository<userImpl> : IUserRepository where userImpl : IUser
    {
        #region Member Variable
        private readonly GraphClient _dbms;
        #endregion

        #region Constructor
        public UserRepository ()
        {
             string neo4jLocation  = ConfigurationManager.AppSettings["Neo4jLocation"];
             _dbms = new GraphClient(new Uri(neo4jLocation));
             _dbms.Connect();
        }
        #endregion

        #region Public Methods
        public void Add(IUser poco)
        {
            _dbms.Cypher.Merge("(user:User { ID: {ID} })").OnCreate().Set("user = {poco}").WithParams(new { ID = poco.ID, poco }).ExecuteWithoutResults();
        }
        public void Remove(IUser poco)
        {
            _dbms.Cypher.Match("(user:User)").Where((userImpl user) => user.ID == poco.ID).
                  OptionalMatch("(user:User)-[r]-()").Where((userImpl user) => user.ID == poco.ID).Delete("user").ExecuteWithoutResults();
        }
        public IEnumerable<IUser> Find(System.Linq.Expressions.Expression<Func<IUser, bool>> predicate)
        {
            var query = _dbms.Cypher.Match("(user:User)").Where(predicate).Return(user => user.As<userImpl>());
            var ienum = query.Results;
            
            List<IUser> list = new List<IUser>();
            foreach (IUser usr in ienum)
                list.Add(usr);

            return list;
        }
        public void AddToContactList(IUser usr, IUser friend)
        {
            _dbms.Cypher.Match("(user1:User)", "(user2:User)").Where((User user1) => user1.ID.ToString() == usr.ID.ToString())
                 .AndWhere((User user2) => user2.ID.ToString() == friend.ID.ToString()).CreateUnique("user1-[:FRIENDS_WITH]->user2")
                 .ExecuteWithoutResults();
        }

        public void AddFriendToGroup(IUser user, IGroup grp, IUser friend)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
