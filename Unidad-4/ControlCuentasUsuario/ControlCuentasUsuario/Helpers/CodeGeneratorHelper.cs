using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlCuentasUsuario.Helpers
{
    public class CodeGeneratorHelper
    {
        public static int GetCode()
        {
            Random r = new Random();
            int code = r.Next(1000, 9999);
            int code2 = r.Next(1000, 9999);
            return (code + code2);
        }
    }
}
