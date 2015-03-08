using urTribeWebAPI.Common.Interfaces;


namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IUserRepository : IRepository<IUser>
    {
        void Add(IUser usr);
        void AddToContactList(IUser usr, IUser friend);
        void AddFriendToGroup(IUser user, IGroup grp, IUser friend);
    }
}
