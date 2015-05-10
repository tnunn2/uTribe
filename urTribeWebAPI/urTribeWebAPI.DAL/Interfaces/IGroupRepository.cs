using urTribeWebAPI.Common;

namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IGroupRepository : IRepository<IGroup>
    {
        void Add(IUser usr, IGroup grp);
    }
}
