using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actividad_5.Models;

namespace Actividad_5.Repository
{
    public abstract class Repository<T> where T : class
    {
        public animalesContext context { get; set; }
        public Repository(animalesContext c)
        {
            context = c;
        }
        public virtual IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }
        public virtual T GetById(object id)
        {
            return context.Find<T>(id);
        }
        public virtual void Insert(T entity)
        {
            if (Validate(entity))
            {
                context.Add(entity);
                Save();
            }
        }
        public virtual void Update(T entity)
        {
            if (Validate(entity))
            {
                context.Update(entity);
                Save();
            }
        }
        public virtual void Delete(T entity)
        {
            context.Remove(entity);
            Save();
        }
        public virtual void Save()
        {
            context.SaveChanges();
        }

        public virtual bool Validate(T entity)
        {
            return true;
        }
    }
}
