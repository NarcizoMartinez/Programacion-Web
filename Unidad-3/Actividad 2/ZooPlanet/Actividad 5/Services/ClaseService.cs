using Actividad_5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actividad_5.Services
{
    public class ClaseService
    {
        public List<Clase> Clases { get; set; }
        public ClaseService()
        {
            using (animalesContext context= new animalesContext())
            {
                Clases = context.Clase.OrderBy(x => x.Nombre).ToList();
            }
        }
    }
}
