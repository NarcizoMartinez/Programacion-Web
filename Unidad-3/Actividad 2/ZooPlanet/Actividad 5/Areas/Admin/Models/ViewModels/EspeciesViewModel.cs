using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actividad_5.Models;

namespace Actividad_5.Admin.Models.ViewModels
{
    public class EspeciesViewModel
    {
        public IEnumerable<Clase> Clases { get; set; }
        public Especies Especies { get; set; }
        public IFormFile Archivo { get; set; }
        public string Imagen { get; set; }
    }
}
