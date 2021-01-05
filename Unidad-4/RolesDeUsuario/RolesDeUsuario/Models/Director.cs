using System;
using System.Collections.Generic;

namespace RolesDeUsuario.Models
{
    public partial class Director
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ncontrol { get; set; }
        public string Contrasena { get; set; }
    }
}
