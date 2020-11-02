using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actividad_5.Models;
using System.IO;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace Actividad_5.Repository
{
    public class EspecieRepository : Repository<Especies>
    {
      public EspecieRepository(animalesContext context) : base(context)
        {

        }
        public override IEnumerable<Especies> GetAll()
        {
            return base.GetAll().OrderBy(x => x.Especie);
        }
        public IEnumerable<Especies> GetEspeciesByClase(string id)
        {
            return context.Especies.Include(x => x.IdClaseNavigation).Where(x => x.IdClaseNavigation.Nombre == id).OrderBy(x => x.Especie);
        }
        public override Especies GetById(object id)
        {
            return context.Especies.Include(x=>x.IdClaseNavigation).FirstOrDefault(x => x.Id == (int)id);
        }
        public override bool Validate(Especies esp)
        {
            if (string.IsNullOrWhiteSpace(esp.Especie))
            {
                throw new Exception("Por favor introduzca el nombre de la especie.");
            }
            if (string.IsNullOrWhiteSpace(esp.Habitat))
            {
                throw new Exception("Por favor introduzca el nombre del habitat.");
            }
            if (esp.Tamaño==null||esp.Tamaño<=0)
            {
                throw new Exception("Por favor introduzca el tamaño de la especie.");
            }
            if (esp.Peso == null || esp.Peso <= 0)
            {
                throw new Exception("Por favor introduzca el nombre de la especie.");
            }
            if (string.IsNullOrWhiteSpace(esp.Observaciones))
            {
                throw new Exception("Por favor introduzca las observaciones sobre la especie.");
            }
            if (context.Especies.Any(x=>x.Especie==esp.Especie&&x.Id!=esp.Id))
            {
                throw new Exception("La especie ya se encuentra agregada.");
            }
            return true;
        }
    }
}
