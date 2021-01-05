using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RolesDeUsuario.Models;
using RolesDeUsuario.ViewModels;
using RolesDeUsuario.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RolesDeUsuario.Helpers;

namespace RolesDeUsuario.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LoginDirector()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginDirector(Director _director)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            RolUsuarioRepository<Director> repos = new RolUsuarioRepository<Director>(_context);
            var dir = _context.Director.FirstOrDefault(x => x.Ncontrol == _director.Ncontrol);
            try
            {
                if (dir!=null&&dir.Contrasena==HashHelper.GetHash(dir.Contrasena))
                {
                    List<Claim> datos = new List<Claim>();
                    datos.Add(new Claim(ClaimTypes.Name, dir.Nombre));
                    datos.Add(new Claim("Numero Control",dir.Ncontrol.ToString()));
                    datos.Add(new Claim(ClaimTypes.Role, "Director"));
                    datos.Add(new Claim("Nombre", dir.Nombre));
                    var claimIdentity = new ClaimsIdentity(datos, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimMain = new ClaimsPrincipal(claimIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimMain,
                    new AuthenticationProperties { IsPersistent = true });
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "El numero de control o la contrasena es incorrecta.");
                    return View(_director);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(_director);
            }
        }
    }
}
