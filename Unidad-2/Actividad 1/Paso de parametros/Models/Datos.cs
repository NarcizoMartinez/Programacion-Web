using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paso_de_parametros.Models
{
    public class Datos
    {
        public int d1 { get; set; }
        public int d2 { get; set; }
        public int Resultado()
        {
            return d1 + d2;
        }
    }
}
