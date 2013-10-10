using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sfs.Services;

namespace Sfs.Controllers
{
    public class HomeController : CustomController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult VerLista(Guid id)
        {
            var atividade = Context.Atividades.Find(id);
            atividade.Inscricoes.Sort((p1,p2) => -p1.CoeficienteSorte.CompareTo(-p2.CoeficienteSorte));
            //atividade.Inscricoes = ServicoAtividade.SortListas(Context, id);
            return View(atividade);
        }
    }
}