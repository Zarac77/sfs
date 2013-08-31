using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using Sfs.Models;

namespace Sfs.Controllers
{
    public class GerenciamentoBDController : CustomController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AlimentarBD(HttpPostedFileBase source)
        {
            var stream = source.InputStream;
            byte[] buf = new byte[stream.Length];
            if (stream.Length < int.MaxValue)
            {
                stream.Read(buf, 0, (int)stream.Length);
            }
            var textoFile = Encoding.UTF8.GetString(buf);
            StringReader reader = new StringReader(textoFile);
            string linha = reader.ReadLine();
            while (linha != null)
            {
                string[] dados = linha.Split(';');
                Pessoa p = new Pessoa
                {
                    Id = Guid.NewGuid(),
                    Nome = dados[0],
                    Matricula = dados[1],
                    Turma = dados[2]
                };
                Context.Pessoas.Add(p);
                linha = reader.ReadLine();
            }

            Context.SaveChanges();

            return RedirectToAction("Index");
            //return RedirectToAction("Index");
        }
    }
}
