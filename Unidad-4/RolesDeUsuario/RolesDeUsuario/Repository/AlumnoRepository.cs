using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RolesDeUsuario.Models;

namespace RolesDeUsuario.Repository
{
    public class AlumnoRepository:RolUsuarioRepository<Alumno>
    {
        public AlumnoRepository(rolesusuarioContext _context):base(_context)
        {

        }
        public Alumno GetByNControl(string ncontrol)
        {
            return Context.Alumno.FirstOrDefault(x => x.Ncontrol == ncontrol);
        }
        public override bool Validate(Alumno entity)
        {
            if (string.IsNullOrEmpty(entity.Ncontrol))
                throw new Exception("Por favor asigne un numero de control.");
            if (string.IsNullOrEmpty(entity.Nombre))
                throw new Exception("Por favor asigne un nombre.");
            if (entity.IdMaestro <= 0 || entity.IdMaestro == null)
                throw new Exception("Debe asignar el alumno a un Maestro.");
            return true;
        }
    }
}
