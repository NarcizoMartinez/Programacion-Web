using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Paso_de_parametros.Models;
namespace Paso_de_parametros.Controllers
{
    public class Suma : Controller
    {
        [HttpPost]
        public IActionResult Index(Datos s)
        {
            return View(s);
        }
        public IActionResult Capturar()
        {
            return View();
        }
    }
}
