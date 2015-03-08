using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IGroupRepository : IRepository<IGroup>
    {
        void Add(IUser usr, IGroup grp);
    }
}
