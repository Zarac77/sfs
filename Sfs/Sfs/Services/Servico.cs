using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.Services
{
    public class Servico
    {
        public static bool AdicionarEntry(object entry, ref List<object> lista)
        {
            lista.Add(entry);
            return true;
        }

    }
}