using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlCuentasUsuario.Models;

namespace ControlCuentasUsuario.Repository
{
    public class UserRepository<T> where T : class
    {
        public usuariosContext Context { get; set; }
        public UserRepository(usuariosContext _context)
        {
            Context = _context;
        }
        public Cuentum GetById(int id)
        {
            return Context.Cuenta.FirstOrDefault(x => x.IdUser == id);
        }
        public Cuentum GetByEmail(string email)
        {
            return Context.Cuenta.FirstOrDefault(x => x.Email == email);
        }
        public Cuentum GetByUser(Cuentum id)
        {
            return Context.Find<Cuentum>(id);
        }
        public bool Validar(Cuentum user)
        {
            if (string.IsNullOrEmpty(user.Username)) throw new Exception("Por favor ingrese el nombre de usuario.");
            if (string.IsNullOrEmpty(user.Email)) throw new Exception("Por favor ingrese el correo electronico.");
            if (string.IsNullOrEmpty(user.Password)) throw new Exception("Por favor ingrese la contrasena.");
            return true;
        }
        public virtual void Insert(Cuentum user)
        {
            if (Validar(user))
            {
                Context.Add(user);
                Context.SaveChanges();
            }
        }
        public virtual void Edit(Cuentum user)
        {
            if (Validar(user))
            {
                Context.Update<Cuentum>(user);
                Context.SaveChanges();
            }
        }
        public virtual void Delete(Cuentum user)
        {
            if (Validar(user))
            {
                Context.Remove<Cuentum>(user);
                Context.SaveChanges();
            }
        }
    }

}
