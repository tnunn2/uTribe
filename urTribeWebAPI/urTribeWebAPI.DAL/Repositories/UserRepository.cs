using System;
using System.Collections.Generic;
using Neo4jClient;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Common.Concrete;
using urTribeWebAPI.DAL.Interfaces;

namespace urTribeWebAPI.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Member Variable
        private readonly GraphClient _dbms;
        #endregion

        #region Constructor
        public UserRepository ()
        {
            _dbms = new GraphClient(new Uri("http://localhost:7474/db/data"));
            _dbms.Connect();
        }
        #endregion

        #region Public Methods
        public void Add(IUser poco)
        {
            var query = _dbms.Cypher.Match("(user:User)").Where((User user) => user.ID == poco.ID).Return(user => user.As<User>()).Limit(1);

            if (((List<User>)query.Results).Count == 0)
              _dbms.Cypher.Create("(user:User {poco})").WithParam("poco", poco).ExecuteWithoutResults();
        }

        public void Remove(IUser poco)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUser> Find(System.Linq.Expressions.Expression<Func<IUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
