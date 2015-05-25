using System;
using System.Collections.Generic;
using Neo4jClient;
using urTribeWebAPI.Common;
using urTribeWebAPI.DAL.Interfaces;
using System.Configuration;

namespace urTribeWebAPI.DAL.Repositories
{
    public class GroupRepository<groupImpl> : IGroupRepository where groupImpl : IGroup
    {
        #region Member Variable
        private readonly GraphClient _dbms;
        #endregion

        #region Constructor
        public GroupRepository()
        {
             string neo4jLocation  = Properties.Settings.Default.Neo4jLocation;
             _dbms = new GraphClient(new Uri(neo4jLocation));
             _dbms.Connect();
        }
        #endregion

        #region Public Methods
        public void Add(IUser usr, IGroup grp)
        {
            _dbms.Cypher.Match("(owner:User)")
                .Where((User owner) => owner.ID.ToString() == usr.ID.ToString()).Create("owner-[:GROUPOWNER]->(group:Group {grp})")
                .WithParam("grp", grp).ExecuteWithoutResults();
        }

        public void Remove(IGroup poco)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGroup> Find(System.Linq.Expressions.Expression<Func<IGroup, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
