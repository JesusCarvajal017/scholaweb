using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.interfaces.crud
{
    public interface IQueryByIdLinq<T>
    {
        Task<T> GetByIdAsyncLinq(int id);
    }
}
