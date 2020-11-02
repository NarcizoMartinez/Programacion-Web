using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actividad_5.Models;
namespace Actividad_5.Repository
{
    public class ClaseRepository : Repository<Clase>
    {
        public ClaseRepository(animalesContext context) : base(context)
        {

        }
        public override IEnumerable<Clase> GetAll()
        {
            return context.Clase.OrderBy(x => x.Nombre);
        }
    }
}
