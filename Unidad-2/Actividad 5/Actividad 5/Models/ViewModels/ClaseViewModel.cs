using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actividad_5.Models.ViewModels
{
    public class ClaseViewModel
    {
        public string NombreClase { get; set; }
        public IEnumerable<Models.Clase> Clases { get; set; }
    }
}
