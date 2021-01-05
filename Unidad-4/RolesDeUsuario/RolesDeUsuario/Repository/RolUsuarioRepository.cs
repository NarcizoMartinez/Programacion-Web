using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RolesDeUsuario.Models;

namespace RolesDeUsuario.Repository
{
    public class RolUsuarioRepository<T> where T:class
    {
        public rolesusuarioContext Context { get; set; }
        public RolUsuarioRepository(rolesusuarioContext _context)
        {
            Context = _context;
        }
        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }
        public T GetTById(object id)
        {
            return Context.Find<T>(id);
        }
        public virtual void Insert(T entity)
        {
            if (Validate(entity))
            {
                Context.Update<T>(entity);
                Context.SaveChanges();
            }
        }
        public void Delete(T entity)
        {
            Context.Remove<T>(entity);
            Context.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            if (Validate(entity))
            {
                Context.Update<T>(entity);
                Context.SaveChanges();
            }
        }
        public virtual bool Validate(T entity)
        {
            return true;
        }
    }
}
