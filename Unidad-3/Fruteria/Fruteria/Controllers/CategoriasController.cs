using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fruteria.Models;
using Fruteria.Repositories;


namespace Fruteria.Controllers
{
    public class CategoriasController : Controller
    {
        [Route("Categorias")]
        public IActionResult Index()
        {
            fruteriashopContext context = new fruteriashopContext();
            Repository<Categorias> repos = new Repository<Categorias>(context);
            /// RETURN PARA ELIMINACIÓN FISICA
            //return View(repos.GetAll().OrderBy(x => x.Nombre));
            // RETURN PARA ELIMINACION LOGICA
            return View(repos.GetAll().Where(x => x.Eliminado == false).OrderBy(x => x.Nombre));


        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(Categorias c)
        {
            try
            {
                fruteriashopContext context = new fruteriashopContext();
                CategoriasRepository repos = new CategoriasRepository(context);
                repos.Insert(c);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(c);
            }

        }

        public IActionResult Editar(int id)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                CategoriasRepository repos = new CategoriasRepository(context);

                var categoria = repos.Get(id);
                if (categoria == null)
                {
                    return RedirectToAction("Index");
                }

                return View(categoria);
            }
        }

        [HttpPost]
        public IActionResult Editar(Categorias c)
        {
            try
            {
                using (fruteriashopContext context = new fruteriashopContext())
                {
                    CategoriasRepository repos = new CategoriasRepository(context);
                    var original = repos.Get(c.Id);
                    original.Nombre = c.Nombre;
                    repos.Update(original);
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(c);
            }
        }

        public IActionResult Eliminar(int id)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                CategoriasRepository repos = new CategoriasRepository(context);
                var categoria = repos.Get(id);

                if (categoria == null)
                {
                    return RedirectToAction("Index");
                }
                else
                    return View(categoria);

            }
        }

        [HttpPost]
        public IActionResult Eliminar(Categorias c)
        {
            try
            {
                using (fruteriashopContext context = new fruteriashopContext())
                {
                    CategoriasRepository repos = new CategoriasRepository(context);
                    var categoria = repos.Get(c.Id);
                    categoria.Eliminado = true;
                    repos.Update(categoria);
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(c);
            }

        }
    }
}
