using System;
using System.Collections.Generic;
using Neo4jClient;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Common.Concrete;
using urTribeWebAPI.DAL.Interfaces;
using System.Configuration;

namespace urTribeWebAPI.DAL.Repositories
{
    public class EventRepository<eventImpl> : IEventRepository where eventImpl : IEvent
    {
        #region Member Variable
        private readonly GraphClient _dbms;
        #endregion

        #region Constructor
        public EventRepository()
        {
             string neo4jLocation  = ConfigurationManager.AppSettings["Neo4jLocation"];
             _dbms = new GraphClient(new Uri(neo4jLocation));
             _dbms.Connect();
        }
        #endregion

        #region Public Methods
        public void Add(IUser usr, IEvent evt)
        {
            _dbms.Cypher.Match("(inviter:User)")
                .Where((User inviter) => inviter.ID.ToString() == usr.ID.ToString()).Create("inviter-[:EVENTOWNER]->(event:Event {evt})")
                .WithParam("evt", evt).ExecuteWithoutResults();
        }
        public void LinkToEvent(IUser usr, IEvent evt)
        {
            _dbms.Cypher
                 .Match("(user:User)", "(event:Event)").Where((User user) => user.ID.ToString() == usr.ID.ToString())
                 .AndWhere((ScheduledEvent schevt) => schevt.ID.ToString() == evt.ID.ToString()).Create("user1-[:Guest]->event")
                 .ExecuteWithoutResults();
        }
        public void Remove(IEvent poco)
        {
            _dbms.Cypher.Match("(evt:Event)").Where((eventImpl evt) => evt.ID == poco.ID).
                  OptionalMatch("(evt:Event)-[r]-()").Where((eventImpl evt) => evt.ID == poco.ID).Delete("evt").ExecuteWithoutResults();
        }
        public IEnumerable<IEvent> Find(System.Linq.Expressions.Expression<Func<IEvent, bool>> predicate)
        {
            var query = _dbms.Cypher.Match("(evt:Event)").Where(predicate).Return(evt => evt.As<eventImpl>());
            var ienum = query.Results;
            
            List<IEvent> list = new List<IEvent>();
            foreach (IEvent evt in ienum)
                list.Add(evt);

            return list;
        }
        #endregion
    }

}
