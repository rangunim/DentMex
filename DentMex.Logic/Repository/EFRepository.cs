using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DentMex.Domain;

namespace DentMex.Logic.Repository
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private bazaEntities context = null;

        public EFRepository()
        {
            context = new DentMexContext();
            context.Configuration.LazyLoadingEnabled = false;
        }

        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                context.Set<T>().Include(property).Load();
            }

            return context.Set<T>();
        }

        public virtual IQueryable<T> GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                context.Set<T>().Include(property).Load();
            }

            return context.Set<T>().Where(predicate);
        }

        public T FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                context.Set<T>().Include(property).Load();
            }

            return context.Set<T>().FirstOrDefault(predicate);
        }

        public virtual void Add(T entity)
        {
            var type = entity.GetType();
            var property = type.GetProperty("DateCreated");
            if (property != null)
            {
                property.SetValue(entity, DateTime.UtcNow, null);
            }

            property = type.GetProperty("LastUpdated");
            if (property != null)
            {
                property.SetValue(entity, DateTime.UtcNow, null);
            }

            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            var type = entity.GetType();
            var property = type.GetProperty("LastUpdated");
            if (property != null)
            {
                property.SetValue(entity, DateTime.UtcNow, null);
            }

            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public int Count()
        {
            return context.Set<T>().Count();
        }
    }

}
