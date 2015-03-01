using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IRepository <T> where T : IDBRepositoryObject
    {
        void Add(T poco);
        void Remove(T poco);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
