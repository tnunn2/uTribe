using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IRepository <type> where type : IDBRepositoryObject
    {
        void Remove(type poco);
        IEnumerable<type> Find(Expression<Func<type, bool>> predicate);
    }
}
