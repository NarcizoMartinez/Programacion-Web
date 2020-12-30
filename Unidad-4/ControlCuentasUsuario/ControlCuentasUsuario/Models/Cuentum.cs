using System;
using System.Collections.Generic;

#nullable disable

namespace ControlCuentasUsuario.Models
{
    public partial class Cuentum
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Token { get; set; }
        public ulong? Activo { get; set; }
    }
}
