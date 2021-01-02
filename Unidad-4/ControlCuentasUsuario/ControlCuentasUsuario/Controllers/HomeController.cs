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
        public IWebHostEnvironment Environment { get; set; }
        public HomeController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        [Authorize(Roles = "User")]
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
        public async Task<IActionResult> LogIn(Cuentum user, bool persistent)
        {
            usuariosContext context = new usuariosContext();
            UserRepository<Cuentum> repos = new UserRepository<Cuentum>(context);
            var datos = repos.GetByEmail(user.Email);
            if (datos != null && HashHelper.GetHash(user.Password) == datos.Password)
            {
                if (datos.Active == 1)
                {
                    List<Claim> info = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, datos.Username),
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim("Email", datos.Email),
                        new Claim("Username", datos.Username)
                    };
                    var claimIdentity = new ClaimsIdentity(info, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimMain = new ClaimsPrincipal(claimIdentity);
                    if (persistent == true)
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
                    ModelState.AddModelError("", "Esta cuenta aun no ha sido activada. Por favor verificar en su correo electrónico.");
                    return View(user);
                }
            }
            else
            {
                ModelState.AddModelError("", "El correo o la contraseña son incorrectas.");
                return View(user);
            }
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("LogIn");
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(Cuentum user, string pass, string pass2)
        {
            usuariosContext context = new usuariosContext();
            UserRepository<Cuentum> repos = new UserRepository<Cuentum>(context);
            try
            {
                if (context.Cuenta.Any(x => x.Email == user.Email))
                {
                    ModelState.AddModelError("", "El correo ya se encuentra registrado");
                    return View(user);
                }
                else
                {
                    if (pass == pass2)
                    {
                        user.Password = HashHelper.GetHash(pass);
                        user.Token = CodeGeneratorHelper.GetCode();
                        user.Active = 0;
                        repos.Insert(user);
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("senorvinter@gmail.com", "Discord");
                        message.To.Add(user.Email);
                        message.Subject = "Por favor confirma tu dirección de correo electrónico de Discord";
                        string text = System.IO.File.ReadAllText(Environment.WebRootPath + "/ConfirmEmail.html");
                        message.Body = text.Replace("{##code##}", user.Token.ToString());
                        message.IsBodyHtml = true;

                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                        {
                            EnableSsl = true,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential("senorvinter@gmail.com", "dgrayman5")
                        };
                        client.Send(message);
                        return RedirectToAction("Activate");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Las contraseñas no son iguales.");
                        return View(user);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
               // Console.WriteLine(ex.InnerException.Message);
                return View(user);
            }
        }
        [AllowAnonymous]
        public IActionResult Activate()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Activate(int code)
        {
            usuariosContext context = new usuariosContext();
            UserRepository<Cuentum> repos = new UserRepository<Cuentum>(context);
            var us = context.Cuenta.FirstOrDefault(x => x.Token == code);
            if (us != null && us.Active == 0)
            {
                var c = us.Token;
                if (code == c)
                {
                    us.Active = 1;
                    repos.Edit(us);
                    return RedirectToAction("LogIn");
                }
                else
                {
                    ModelState.AddModelError("", "El token ingresado no coincide.");
                    return View(code);
                }
            }
            else
            {
                ModelState.AddModelError("", "El usuario no existe.");
                return View(code);
            }
        }
        [Authorize(Roles = "User")]
        public IActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult ChangePass(string email, string pass, string newpass, string newpassconfirm)
        {
            usuariosContext context = new usuariosContext();
            UserRepository<Cuentum> repos = new UserRepository<Cuentum>(context);
            try
            {
                var user = repos.GetByEmail(email);
                if (user.Password != HashHelper.GetHash(pass))
                {
                    ModelState.AddModelError("", "La contraseña ingresada es incorrecta.");
                    return View();
                }
                else
                {
                    if (newpass!=newpassconfirm)
                    {
                        ModelState.AddModelError("", "Las nuevas contraseñas no coinciden.");
                        return View();
                    }
                    else if (user.Password== HashHelper.GetHash(newpass))
                    {
                        ModelState.AddModelError("", "La nueva contraseña no puede ser igual a la anterior");
                        return View();
                    }
                    else
                    {
                        user.Password = HashHelper.GetHash(newpass);
                        repos.Edit(user);
                        return RedirectToAction("LogIn");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        [AllowAnonymous]
        public IActionResult RecoverPass()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult RecoverPass(string email)
        {
            try
            {
                usuariosContext context = new usuariosContext();
                UserRepository<Cuentum> repos = new UserRepository<Cuentum>(context);
                var user = repos.GetByEmail(email);
                if (user!=null)
                {
                    var temp = CodeGeneratorHelper.GetCode();
                    MailMessage message = new MailMessage("senorvinter@gmail.com", "Discord");
                    message.From = new MailAddress("senorvinter@gmail.com", "Discord");
                    message.To.Add(email);
                    message.Subject = "Recupera tu contraseña de Discord";
                    string text = System.IO.File.ReadAllText(Environment.WebRootPath + "/Recover.html");
                    message.Body = text.Replace("{##codeTemp##}", temp.ToString());
                    message.IsBodyHtml = true;

                    SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587)
                    {
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("senorvinter@gmail.com", "dgrayman5")
                    };
                    cliente.Send(message);
                    user.Password = HashHelper.GetHash(temp.ToString());
                    repos.Edit(user);
                    return RedirectToAction("LogIn");
                }
                else
                {
                    ModelState.AddModelError("", "El correo" +email+ "no se encuentra registrado.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View((object)email);
            }
        }
        [Authorize(Roles ="User")]
        public IActionResult DeleteAccount()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult DeleteAccount(string email, string password)
        {
            try
            {
                usuariosContext context = new usuariosContext();
                UserRepository<Cuentum> repos = new UserRepository<Cuentum>(context);
                var user = repos.GetByEmail(email);
                if (user!=null)
                {
                    if (HashHelper.GetHash(password)==user.Password)
                    {
                        repos.Delete(user);
                    }
                    else
                    {
                        ModelState.AddModelError("", "La contraseña esta incorrecta.");
                        return View();
                    }
                }
                return RedirectToAction("LogIn");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ha ocurrido un error, por favor intentelo de nuevo en un par de minutos.");
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult Denied()
        {
            return View();
        }
    }
}
