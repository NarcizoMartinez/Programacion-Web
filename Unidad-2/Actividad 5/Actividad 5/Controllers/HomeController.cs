using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Actividad_5.Models;
using Actividad_5.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Actividad_5.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            animalesContext context = new animalesContext();
            var cl= context.Clase.OrderBy(x => x.Nombre).ToList();
            return View(cl);
        }
        public IActionResult Clase()
        {
           // ClaseViewModel vm = new ClaseViewModel();
            animalesContext context = new animalesContext();
            var clases = context.Especies.Include(x => x.Especie).Select(x=> new ClaseViewModel { NombreClase=x.Especie});

            return View(clases);
        }
    }
}
