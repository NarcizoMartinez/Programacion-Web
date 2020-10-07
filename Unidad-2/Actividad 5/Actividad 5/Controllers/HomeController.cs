using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Actividad_5.Models;
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
        [Route("Clase/{id}")]
        public IActionResult Clase(string Id)
        {
            animalesContext context = new animalesContext();
            var n = Id.Replace('-', ' ').ToUpper();
            var clases = context.Clase.Include(x => x.Especies).Where(x => x.Nombre.ToUpper() == n.ToUpper()).OrderBy(x => x.Nombre);
            if (clases.Count() == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(clases);
            }  
        }
    }
}
