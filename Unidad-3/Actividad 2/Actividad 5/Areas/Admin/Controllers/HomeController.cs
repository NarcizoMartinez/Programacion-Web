using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Actividad_5.Models;
using Actividad_5.Repository;
using Actividad_5.Admin.Models.ViewModels;

namespace Actividad_5.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IWebHostEnvironment WebHost { get; set; }
        animalesContext context;
        public HomeController(IWebHostEnvironment webHost, animalesContext c)
        {
            WebHost = webHost;
            context = c;
        }
        public IActionResult Index()
        {
            EspecieRepository repos = new EspecieRepository(context);
            var listEsp = repos.GetAll();
            return View(listEsp);
        }
        public IActionResult Agregar()
        {
            EspeciesViewModel vm = new EspeciesViewModel();
            ClaseRepository clase = new ClaseRepository(context);
            vm.Clases = clase.GetAll();
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(EspeciesViewModel vm)
        {
            try
            {
                EspecieRepository repository = new EspecieRepository(context);
                repository.Insert(vm.Especies);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ClaseRepository clase = new ClaseRepository(context);
                vm.Clases = clase.GetAll();
                return View(vm);
            }
        }
        public IActionResult Editar(int id)
        {
            EspeciesViewModel vm = new EspeciesViewModel();
            EspecieRepository esp = new EspecieRepository(context);
            vm.Especies = esp.GetById(id);
            if (vm.Especies==null)
            {
                return RedirectToAction("Index");
            }
            ClaseRepository claserepo = new ClaseRepository(context);
            vm.Clases = claserepo.GetAll();
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(EspeciesViewModel vm)
        {
            try
            {
                EspecieRepository esp = new EspecieRepository(context);
                var espre = esp.GetById(vm.Especies.Id);
                if (espre!=null)
                {
                    espre.Especie = vm.Especies.Especie;
                    espre.Habitat = vm.Especies.Habitat;
                    espre.IdClase = vm.Especies.IdClase;
                    espre.Observaciones = vm.Especies.Observaciones;
                    espre.Peso = vm.Especies.Peso;
                    espre.Tamaño = vm.Especies.Tamaño;
                    esp.Update(espre);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ClaseRepository clase = new ClaseRepository(context);
                vm.Clases = clase.GetAll();
                return View(vm);
            }
        }
        public IActionResult Eliminar(int id)
        {
            EspecieRepository esp = new EspecieRepository(context);
            if (esp!= null)
            {
                return View(esp);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Eliminar(Especies especies)
        {
            EspecieRepository esp = new EspecieRepository(context);
            var espre = esp.GetById(especies.Id);
            if (espre != null)
            {
                esp.Delete(espre);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "La especie ya ha sido eliminada.");
                return View(especies);
            }
        }
        public IActionResult Img(int id)
        {
            EspeciesViewModel vm = new EspeciesViewModel();
            EspecieRepository esp = new EspecieRepository(context);
            vm.Especies = esp.GetById(id);
            if (vm.Especies == null)
            {
                return RedirectToAction("Index");
            }
            if (System.IO.File.Exists(WebHost.WebRootPath+ $"/especies/{vm.Especies.Id}.jpg"))
            {
                vm.Imagen = $"{vm.Especies.Id}.jpg";
            }
            else
            {
                vm.Imagen = "nophoto.jpg";
            }
            return View(vm);
        }
        [HttpPost]  
        public IActionResult Img(EspeciesViewModel vm)
        {
            try
            {
                if (vm.Archivo == null)
                {
                    ModelState.AddModelError("", "Por favor seleccione una imagen");
                    return View(vm);

                }
                else
                {
                    if (vm.Archivo.ContentType!="image/jpeg"||vm.Archivo.Length>1024 * 1024 * 2)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un archivo jpeg menor a 2Mb.");
                        return View(vm);
                    }
                    FileStream fs = new FileStream(WebHost.WebRootPath + $"/especies/{vm.Especies.Id}.jpg", FileMode.Create);
                    vm.Archivo.CopyTo(fs);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }
    }
}
