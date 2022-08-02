using EClassField.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace EClassField.Data
{
    public class Repository<T> : Core.Repository<T> where T : class
    {

        ClassFieldDbContext ctx = new ClassFieldDbContext();

        public T Add(T entity)
        {

            if (entity != null)
            {
                ctx.Set<T>().Add(entity);
                Update();
            }
            return entity;

        }

        public IQueryable<T> AsNoTracking()
        {
            return ctx.Set<T>().AsNoTracking();
        }

        public T Delete(T entity)
        {
            if (entity != null)
            {
                ctx.Set<T>().Remove(entity);
                Update();
            }
            return entity;
        }

        public T GetById(Expression<Func<T, bool>> expression)
        {
            return ctx.Set<T>().FirstOrDefault(expression);
        }

        public T GetById(object id)
        {
            return null;
        }

        public IList<T> GetList(Expression<Func<T, bool>> expression)
        {
            return ctx.Set<T>().Where(expression).ToList();
        }

        public IQueryable<T> GetTable()
        {
            return ctx.Set<T>().AsQueryable();
        }

        public void Update()
        {
            ctx.SaveChanges();
        }

        public void Update(T entity)
        {
            ctx.SaveChanges();
        }
    }
}
