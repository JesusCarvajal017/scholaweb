using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.interfaces.crud;

namespace Data.interfaces
{
    public interface CrudBase<T> : IQueryLinq<T>, IInsertLinq<T>, IUpdateLing<T>, IDeleteLinq, IQueryByIdLinq<T> where T : class
    {

    }
}
