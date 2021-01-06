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
        [AllowAnonymous]
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
                if (dir!=null&&dir.Contrasena==HashHelper.GetHash(_director.Contrasena))
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
                    return RedirectToAction("Main");
                }
                else
                {
                    ModelState.AddModelError("", "El numero de control o la contraseña es incorrecta.");
                    return View(_director);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(_director);
            }
        }
        [AllowAnonymous]
        public IActionResult LoginMaestro()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginMaestro(Maestro _maestro)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            DocenteRepository repos = new DocenteRepository(_context);
            var ma = repos.GetByNControl(_maestro.Id);
            try
            {
                if (ma!=null&&ma.Contrasena==HashHelper.GetHash(_maestro.Contrasena))
                {
                    if (ma.Activo==1)
                    {
                        List<Claim> datos = new List<Claim>();
                        datos.Add(new Claim(ClaimTypes.Name, ma.Nombre));
                        datos.Add(new Claim("Numero Control",ma.Ncontrol.ToString()));
                        datos.Add(new Claim(ClaimTypes.Role, "Maestro"));
                        datos.Add(new Claim("Nombre",ma.Nombre));
                        datos.Add(new Claim("Id", ma.Id.ToString()));
                        var claimIdentity = new ClaimsIdentity(datos, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimMain = new ClaimsPrincipal(claimIdentity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,claimMain, new AuthenticationProperties { IsPersistent=true});
                        return RedirectToAction("Main", ma.Ncontrol);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Parece ser que su usuario esta desactivado, por favor comuniquese con el administrador para activar su cuenta.");
                        return View(_maestro);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El numero de control o la contrasena son incorrectos.");
                    return View(_maestro);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(_maestro);
                throw;
            }
        }
        [Authorize(Roles = "Director, Maestro")]
        public IActionResult Main(int ncontrol)
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Director")]
        public IActionResult ViewM()
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            DocenteRepository repos = new DocenteRepository(_context);
            var list = repos.GetAll();
            return View(list);
        }
        [Authorize(Roles = "Director")]
        public IActionResult AddM()
        {
            return View();
        }
        [Authorize(Roles = "Director")]
        [HttpPost]
        public IActionResult AddM(Maestro _maestro)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            DocenteRepository repos = new DocenteRepository(_context);
            try
            {
                var v = repos.GetByNControl(_maestro.Ncontrol);
                if (v!=null)
                {
                    ModelState.AddModelError("", "Ya existe un maestro con este numero de control.");
                    return View(_maestro);        
                }
                else
                {
                    _maestro.Activo = 1;
                    _maestro.Contrasena = HashHelper.GetHash(_maestro.Contrasena);
                    repos.Insert(_maestro);
                    return RedirectToAction("Maestro");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(_maestro);
            }
        }
        [Authorize(Roles = "Director")]
        public IActionResult EditM(int id)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            DocenteRepository repos = new DocenteRepository(_context);
            var ma = repos.GetTById(id);
            if (ma==null)
            {
                return RedirectToAction("Maestro");
            }
            return View(ma);
        }
        [Authorize(Roles = "Director")]
        [HttpPost]
        public IActionResult EditM(Maestro _maestro)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            DocenteRepository repos = new DocenteRepository(_context);
            var ma = repos.GetTById(_maestro.Id);
            try
            {
                if (ma!=null)
                {
                    ma.Nombre = _maestro.Nombre;
                    repos.Update(ma);   
                }
                return RedirectToAction("Maestro");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(ma);    
            }
        }
        [Authorize(Roles = "Director")]
        public IActionResult ChangePassword(int id)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            DocenteRepository repos = new DocenteRepository(_context);
            var ma = repos.GetTById(id);
            if (ma==null)
            {
                return RedirectToAction("Maestros");
            }
            return View(ma);
        }
        [Authorize(Roles = "Director")]
        [HttpPost]
        public IActionResult ChangePassword(Maestro _maestro, string pass, string newpass)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            DocenteRepository repos = new DocenteRepository(_context);
            var ma = repos.GetTById(_maestro.Id);
            try
            {
                if (ma!=null)
                {
                    if (pass==newpass)
                    {
                        ma.Contrasena = pass;
                        ma.Contrasena = HashHelper.GetHash(pass);
                        repos.Update(ma);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Las contrasenas no coinciden");
                        return View(ma);
                    }
                }
                return RedirectToAction("Maestro");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(ma);
            }
        }
        [HttpPost]
        public IActionResult LA(int id)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            DocenteRepository reposM = new DocenteRepository(_context);
            var ma = reposM.GetAlumnoByMaestro(id);
            if (ma!=null)
            {
                if (User.IsInRole("Maestro"))
                {
                    if (User.Claims.FirstOrDefault(x => x.Type == "IdMaestro").Value == ma.Id.ToString())
                    {
                        return View(ma);
                    }
                    else
                    {
                        return RedirectToAction("Denied");
                    }
                }
                else if (ma.Activo!=1)
                {
                    return RedirectToAction("Maestro");
                }
                else
                {
                    return View(ma);
                }
            }
            else
            {
                return RedirectToAction("Maestro");
            }
        }
        [Authorize(Roles = "Director, Maestro")]
        public IActionResult AddA(int id)
        {
            AlumnoViewModel _avm = new AlumnoViewModel();
            if (_avm.Maestro!=null)
            {
                if (User.IsInRole("Maestro"))
                {
                    return View(_avm);
                }
                else if (_avm.Maestro.Activo!=1)
                {
                    return RedirectToAction("Maestro");
                }
                else
                {
                    return View(_avm);
                }
            }
            return View(_avm);
        }
        [Authorize(Roles = "Director, Maestro")]
        [HttpPost]
        public IActionResult AddA(AlumnoViewModel _avm)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            AlumnoRepository reposA = new AlumnoRepository(_context);
            DocenteRepository reposM = new DocenteRepository(_context);
            try
            {
                if (_context.Alumno.Any(x=>x.Ncontrol==_avm.Alumno.Ncontrol))
                {
                    ModelState.AddModelError("", "Ya hay un alumno con el mismo numero de control.");
                    return View(_avm);
                }
                else
                {
                    var ma = reposM.GetByNControl(_avm.Maestro.Ncontrol).Id;
                    _avm.Alumno.IdMaestro = ma;
                    reposA.Insert(_avm.Alumno);
                    return RedirectToAction("Alumno", new { id=ma});
                }
            }
            catch (Exception ex)
            {
                _avm.Maestro = reposM.GetTById(_avm.Maestro.Id);
                _avm.Maestro = (Maestro)reposA.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(_avm);
            }
        }
        [Authorize(Roles = "Director, Maestro")]
        public IActionResult EditA(int id)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            AlumnoRepository reposA = new AlumnoRepository(_context);
            DocenteRepository reposM = new DocenteRepository(_context);
            AlumnoViewModel _avm = new AlumnoViewModel();
            _avm.Alumno = reposA.GetTById(id);
            _avm.Maestro = reposM.GetTById(id);
            if (_avm.Alumno!=null)
            {
                _avm.Maestro = reposM.GetTById(_avm.Alumno.IdMaestro);
                if (User.IsInRole("Maestro"))
                {
                    _avm.Maestro = reposM.GetTById(_avm.Alumno.IdMaestro);
                    if (User.Claims.FirstOrDefault(x=>x.Type=="Ncontrol").Value== _avm.Maestro.Ncontrol.ToString())
                    {
                        return View(_avm);
                    }
                    else
                    {
                        return RedirectToAction("Denied");
                    }
                }
                else if (_avm.Maestro.Activo!=1)
                {
                    return RedirectToAction("Maestro");
                }
                else
                {
                    return View(_avm);
                }
            }
            else
            {
                return RedirectToAction("Main");
            }
        }
        [Authorize(Roles = "Director, Maestro")]
        [HttpPost]
        public IActionResult EditA(AlumnoViewModel _avm)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            AlumnoRepository reposA = new AlumnoRepository(_context);
            DocenteRepository reposM = new DocenteRepository(_context);
            try
            {
                var a = reposA.GetTById(_avm.Alumno.Id);
                if (a!=null)
                {
                    a.Nombre = _avm.Alumno.Nombre;
                    if (User.IsInRole("Director"))
                    {
                        a.IdMaestro = _avm.Alumno.IdMaestro;
                    }
                    reposA.Update(a);
                    return RedirectToAction("Alumnos", new { id = a.IdMaestro });
                }
                else
                {
                    ModelState.AddModelError("", "No se ha encontrado al alumno.");
                    _avm.Maestro = reposM.GetTById(_avm.Alumno.IdMaestro);
                    _avm.Maestro = (Maestro)reposM.GetAll();
                    return View(_avm);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _avm.Maestro = reposM.GetTById(_avm.Alumno.IdMaestro);
                _avm.Maestro = (Maestro)reposM.GetAll();
                return View(_avm);
            }
        }
        [HttpPost]
        public IActionResult Deactivate(Maestro _maestro)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            DocenteRepository repos = new DocenteRepository(_context);
            var mad = repos.GetTById(_maestro.Id);
            if (mad!=null&&mad.Activo==1)
            {
                mad.Activo = 0;
                repos.Update(mad);
            }
            else
            {
                mad.Activo = 1;
                repos.Update(mad);
            }
            return RedirectToAction("Maestro");
        }

        [Authorize(Roles = "Director, Maestro")]
        [HttpPost]
        public IActionResult DeleteA(Alumno _alumno)
        {
            rolesusuarioContext _context = new rolesusuarioContext();
            AlumnoRepository repos = new AlumnoRepository(_context);
            var ad = repos.GetTById(_alumno.Id);
            if (ad!=null)
            {
                repos.Delete(ad);
            }
            else
            {
                ModelState.AddModelError("", "No se ha encontradoo el alumno.");
            }
            return RedirectToAction("Alumnos", new {id=ad.IdMaestro});
        }
        public IActionResult Denied()
        {
            return View();
        }
    }
}
