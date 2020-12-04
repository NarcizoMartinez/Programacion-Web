using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fruteria.Models;
using Fruteria.Repositories;
using Microsoft.AspNetCore.Hosting;
using Fruteria.Models.ViewModels;
using System.IO;


namespace Fruteria.Controllers
{
    public class ProductosController : Controller
    {
        public IWebHostEnvironment Enviroment { get; set; }
        public ProductosController(IWebHostEnvironment env)
        {
            Enviroment = env;
        }

        public IActionResult Index()
        {
            ProductosIndexViewModel vm = new ProductosIndexViewModel();
            fruteriashopContext context = new fruteriashopContext();
            CategoriasRepository categoriarepos = new CategoriasRepository(context);
            ProductosRepository productosrepos = new ProductosRepository(context);


            int? id = null;

            // AGREGUÉ EL WHERE PARA QUE NO SE MUESTRE LA CATEGORÍA SI FUÉ ELIMINADA DE MANERA LOGICA
            vm.Categorias = categoriarepos.GetAll().Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);
            vm.Productos = productosrepos.GetProductosByCategoria(id);


            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(ProductosIndexViewModel vm)
        {
            fruteriashopContext context = new fruteriashopContext();
            CategoriasRepository categoriarepos = new CategoriasRepository(context);
            ProductosRepository productosrepos = new ProductosRepository(context);

            // AGREGUÉ EL WHERE PARA QUE NO SE MUESTRE LA CATEGORÍA SI FUÉ ELIMINADA DE MANERA LOGICA
            vm.Categorias = categoriarepos.GetAll().Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);

            if (vm.IdCategoria == 0)
            {
                int? id = null;
                vm.Productos = productosrepos.GetProductosByCategoria(id);
            }
            else
            {
                vm.Productos = productosrepos.GetProductosByCategoria(vm.IdCategoria);
            }

            return View(vm);
        }

        public IActionResult Agregar()
        {
            ProductosViewModel vm = new ProductosViewModel();
            fruteriashopContext context = new fruteriashopContext();
            CategoriasRepository repos = new CategoriasRepository(context);
            vm.Categorias = repos.GetAll().Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(ProductosViewModel vm)
        {
            fruteriashopContext context = new fruteriashopContext();
            try
            {
                if (vm.Archivo == null)
                {
                    ModelState.AddModelError("", "Debe seleccionar la imagen del producto.");
                    CategoriasRepository categoriasrepos = new CategoriasRepository(context);
                    vm.Categorias = categoriasrepos.GetAll().Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);
                    return View(vm);
                }
                else
                {
                    if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un archivo jpg de menos de 2MB.");
                        CategoriasRepository repos = new CategoriasRepository(context);
                        vm.Categorias = repos.GetAll().Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);
                        return View(vm);
                    }
                }


                ProductosRepository productosrepos = new ProductosRepository(context);
                productosrepos.Insert(vm.Producto);

                if (vm.Archivo != null)
                {
                    FileStream fs = new FileStream(Enviroment.WebRootPath + "/img_frutas/" + vm.Producto.Id + ".jpg", FileMode.Create);
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CategoriasRepository repos = new CategoriasRepository(context);
                vm.Categorias = repos.GetAll().OrderBy(x => x.Nombre);
                return View(vm);
            }
        }

        public IActionResult Editar(int Id)
        {
            fruteriashopContext context = new fruteriashopContext();
            ProductosRepository repos = new ProductosRepository(context);
            ProductosViewModel vm = new ProductosViewModel();
            vm.Producto = repos.Get(Id);

            if (vm.Producto == null)
            {
                return RedirectToAction("Index");
            }

            CategoriasRepository cr = new CategoriasRepository(context);
            vm.Categorias = cr.GetAll().Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);

            if (System.IO.File.Exists(Enviroment.WebRootPath + $"/img_frutas/{vm.Producto.Id}.jpg"))
            {
                vm.Imagen = vm.Producto.Id + ".jpg";
            }
            else
            {
                vm.Imagen = "no-disponible.png";
            }

            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(ProductosViewModel vm)
        {
            fruteriashopContext context = new fruteriashopContext();

            if (vm.Archivo != null)
            {
                if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
                {
                    ModelState.AddModelError("", "Debe seleccionar un archivo jpg de menos de 2MB.");
                    CategoriasRepository repos = new CategoriasRepository(context);
                    vm.Categorias = repos.GetAll().Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);
                    return View(vm);
                }
            }

            try
            {
                ProductosRepository repos = new ProductosRepository(context);

                var producto = repos.Get(vm.Producto.Id);

                if (producto != null)
                {
                    producto.Nombre = vm.Producto.Nombre;
                    producto.Precio = vm.Producto.Precio;
                    producto.IdCategoria = vm.Producto.IdCategoria;
                    producto.Descripcion = vm.Producto.Descripcion;
                    producto.UnidadMedida = vm.Producto.UnidadMedida;
                    repos.Update(producto);

                    if (vm.Archivo != null)
                    {
                        FileStream fs = new FileStream(Enviroment.WebRootPath + "/img_frutas/" + vm.Producto.Id + ".jpg", FileMode.Create);
                        vm.Archivo.CopyTo(fs);
                        fs.Close();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CategoriasRepository repos = new CategoriasRepository(context);
                vm.Categorias = repos.GetAll().OrderBy(x => x.Nombre);
                return View(vm);
            }
        }

        public IActionResult Eliminar(int Id)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                var p = repos.Get(Id);

                if (p != null)
                {
                    return View(p);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost]
        public IActionResult Eliminar(Productos p)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                var producto = repos.Get(p.Id);

                if (producto != null)
                {
                    repos.Delete(producto);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "El producto no existe o ya fué eliminado");
                    return View();
                }
            }
        }
    }
}
