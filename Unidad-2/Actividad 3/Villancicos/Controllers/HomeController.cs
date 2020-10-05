using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Villancicos.Models;

namespace Villancicos.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            villancicosContext context = new villancicosContext();
            var v = context.Villancico.OrderBy(x => x.Nombre).ToList();
            return View(v);
        }
        public IActionResult Villancico(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            villancicosContext context = new villancicosContext();
            var villancico = context.Villancico.FirstOrDefault(x => x.Id == id);

            if (villancico == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(villancico);
            }
        }
    }
}
