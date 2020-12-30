using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlCuentasUsuario.Controllers
{
    public class HomeController : Controller
    {
        public IWebHostEnviroment Enviroment { get; set; }
        public HomeController(IWebHostEnviroment _enviroment)
        {
            Enviroment = _enviroment;
        }
        [Authorize(Roles="User")]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
    }
}
