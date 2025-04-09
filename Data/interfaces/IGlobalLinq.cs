using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.interfaces
{
    public interface IGlobalLinq<T>
    {
        Task<IEnumerable<T>> GetAllAsyncLinq();
        Task<T> GetByIdAsyncLinq(int id);
        Task<T> CreateAsyncLinq(T Entity);

        Task<bool> UpdateAsyncLinq(T Entity);

        Task<bool> DeletePersistentAsyncLinq(int id);

        Task<bool> DeleteLogicalAsyncLinq(int id);
    }
}
