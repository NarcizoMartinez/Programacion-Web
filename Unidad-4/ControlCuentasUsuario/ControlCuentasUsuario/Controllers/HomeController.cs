using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using ControlCuentasUsuario.Models;
using ControlCuentasUsuario.Helpers;
using ControlCuentasUsuario.Repository;

namespace ControlCuentasUsuario.Controllers
{
    public class HomeController : Controller
    {
        public IWebHostEnvironment Enviroment { get; set; }
        public HomeController(IWebHostEnvironment _enviroment)
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
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(Cuentum user, bool persistent)
        {
            usuariosContext context = new usuariosContext();
            UserRepository<Cuentum> repos = new UserRepository<Cuentum>(context);
            var datos = repos.GetByEmail(user.Email);
            if (datos!=null &&HashHelper.GetHash(user.Password)==datos.Password)
            {
                if (datos.Active==1)
                {
                    List<Claim> info = new List<Claim>();
                    info.Add(new Claim(ClaimTypes.Name, "Usuario" + datos.Username));
                    info.Add(new Claim(ClaimTypes.Role, "User"));
                    info.Add(new Claim("Email", datos.Email));
                    info.Add(new Claim("Username", datos.Username));
                    var claimIdentity = new ClaimsIdentity(info, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimMain = new ClaimsPrincipal(claimMain);
                    if (persistent==true)
                    {
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimMain, new AuthenticationProperties { IsPersistent = true });
                    }
                    else
                    {
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimMain, new AuthenticationProperties { IsPersistent = false });
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Esta cuenta aun no ha sido activada. Por favor verificar en su correo electronico.");
                    return View(user);
                }
            }
            else
            {
                ModelState.AddModelError("","El correo o la contrasena son incorrectas")
            }
        }
    }
}
