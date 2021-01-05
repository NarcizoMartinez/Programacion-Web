using System;
using System.Collections.Generic;

namespace RolesDeUsuario.Models
{
    public partial class Maestro
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Ncontrol { get; set; }
        public string Contrasena { get; set; }
        public ulong? Activo { get; set; }

        public virtual Alumno Alumno { get; set; }
    }
}
