using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urTribeWebAPI.DAL.Interfaces;

namespace urTribeWebAPI.Test.RepositoryMocks
{
    public interface IFakeRepository : IRepository<IFake>
    {
    }
}
