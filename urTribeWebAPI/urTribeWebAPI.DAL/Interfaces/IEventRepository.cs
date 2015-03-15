using urTribeWebAPI.Common.Interfaces;


namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IEventRepository : IRepository<IEvent>
    {
        void Add(IUser usr, IEvent evt);
        void LinkToEvent(IUser usr, IEvent evt);
    }
}
