using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core
{
    public interface Repository<T>
    {

        T Add(T entity);
        T Delete(T entity);
        void Update();
    
        IList<T> GetList(Expression<Func<T, bool>> expression);
       
        T GetById(Expression<Func<T, bool>> expression);

        IQueryable<T> GetTable();

        IQueryable<T> AsNoTracking();
    }
}

