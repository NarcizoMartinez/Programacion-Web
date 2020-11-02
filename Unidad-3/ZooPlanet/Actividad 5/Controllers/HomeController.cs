using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Actividad_5.Models;
using Actividad_5.Repository;
using Microsoft.EntityFrameworkCore;

namespace Actividad_5.Controllers
{
    public class HomeController : Controller
    {
        animalesContext context;
        public HomeController(animalesContext c)
        {
            context = c;
                    
        }
        [Route("/")]
        public IActionResult Index()
        {
            ClaseRepository crepo = new ClaseRepository(context);
            return View(crepo.GetAll().ToList());
        }
        [Route("{Id}")]
        public IActionResult Clase(string Id)
        {
            ViewBag.Clase = Id;
            EspecieRepository esp = new EspecieRepository(context);
            return View(esp.GetEspeciesByClase(Id));
            
        }
    }
}
