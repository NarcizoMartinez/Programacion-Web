using System;
using System.Collections.Generic;

namespace RolesDeUsuario.Models
{
    public partial class Alumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Ncontrol { get; set; }
        public int? IdMaestro { get; set; }

        public virtual Maestro IdNavigation { get; set; }
    }
}
