using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fruteria.Models;
using Fruteria.Repositories;

namespace Fruteria.Services
{
    public class CategoriasService
    {
        public List<Categorias> Categorias { get; set; }

        public CategoriasService()
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                Repository<Categorias> repos = new Repository<Categorias>(context);
                Categorias = repos.GetAll().Where(x => x.Eliminado == false).OrderBy(x => x.Nombre).ToList();
            }
        }
    }
}
