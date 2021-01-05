using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RolesDeUsuario.Models;
namespace RolesDeUsuario.Repository
{
    public class DocenteRepository:RolUsuarioRepository<Maestro>
    {
        DocenteRepository(rolesusuarioContext _context):base(_context)
        {

        }
        public Maestro GetByNControl(int ncontrol)
        {
            return Context.Maestro.FirstOrDefault(x => x.Ncontrol == ncontrol);
        }
        public Maestro GetAlumnoByMaestro(int id)
        {
            return Context.Maestro.Include(x => x.Alumno).FirstOrDefault(x => x.Id == id);
        }
        public override bool Validate(Maestro entity)
        {
            if (string.IsNullOrEmpty(entity.Ncontrol.ToString()))
                throw new Exception("Por favor asigne un numero de control.");
            if (string.IsNullOrEmpty(entity.Nombre))
                throw new Exception("Por favor asigne un nombre.");
            if (string.IsNullOrEmpty(entity.Contrasena))
                throw new Exception("Por favor asigne una contrasena");
            if (entity.Ncontrol.ToString().Length <= 0)
                throw new Exception("El numero de control debe ser mayor a 0.");
            if (entity.Ncontrol.ToString().Length > 11)
                throw new Exception("El numero de control no puede sobrepasar 11 digitos");
            return true;
        }
    }
}
