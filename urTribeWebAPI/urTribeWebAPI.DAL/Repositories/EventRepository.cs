using System;
using System.Collections.Generic;
using Neo4jClient;
using urTribeWebAPI.Common;
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
            EventRelationship rel = new EventRelationship { AttendStatus = EventAttendantsStatus.Attending};

            _dbms.Cypher.Match("(inviter:User)")
                .Where((User inviter) => inviter.ID.ToString() == usr.ID.ToString()).Create("inviter-[rel:EVENTOWNER]->(event:Event {evt})")
                .WithParam("evt", evt)
                .Set("rel = {rel}")
                .WithParam("rel", rel).ExecuteWithoutResults();
        }
        public void Update(IEvent evt)
        {
            _dbms.Cypher.Match("(evtImp:Event)")
                        .Where((eventImpl evtImp) => evtImp.ID.ToString() == evt.ID.ToString())
                        .Set("evtImp = {evt}")
                        .WithParam("evt", evt)
                        .ExecuteWithoutResults();
        }
        public void LinkToEvent(IUser usr, IEvent evt)
        {
            EventRelationship rel = new EventRelationship { AttendStatus = EventAttendantsStatus.Pending};

            _dbms.Cypher
                 .Match("(user:User)", "(evtImp:Event)")
                 .Where((User user) => user.ID.ToString() == usr.ID.ToString())
                 .AndWhere((ScheduledEvent evtImp) => evtImp.ID.ToString() == evt.ID.ToString()).Create("user-[rel:GUEST]->evtImp")
                 .Set("rel = {rel}")
                 .WithParam("rel", rel).ExecuteWithoutResults();
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
        public void ChangeUserAttendStatus (Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            EventRelationship rel = new EventRelationship { AttendStatus = EventAttendantsStatus.Cancel };

            _dbms.Cypher
                 .Match("(user:User)-[rel:GUEST]->(evtImp:Event)")
                 .Where((User user) => user.ID.ToString() == userId.ToString())
                 .AndWhere((ScheduledEvent evtImp) => evtImp.ID.ToString() == eventId.ToString())
                 .Set("rel = {rel}")
                 .WithParam("rel", rel)
                 .ExecuteWithoutResults();
        }
        #endregion
    }

}
