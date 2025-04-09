using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;

namespace Data.interfaces
{
    public interface IGlobalCrud<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T Entity);
        Task<bool> UpdateAsync(T Entity);
        Task<Object> DeletePersistentAsync(int id);
        Task<Object> DeleteLogicalAsync(int id);

        Task<IEnumerable<T>> GetAllAsyncLinq();

    }
}
