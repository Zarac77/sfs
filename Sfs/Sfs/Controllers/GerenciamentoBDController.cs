using System.Web;
using System.Web.Mvc;
using Sfs.Services;

namespace Sfs.Controllers
{
    public class GerenciamentoBDController : CustomController
    {
        public ActionResult Index()
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            return View();
        }

        [HttpPost]
        public ActionResult AlimentarBD(HttpPostedFileBase source)
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            if (ServicoGerenciamentoBD.AlimentarBD(Context, source))
                return RedirectToAction("ConfirmacaoSucessoAlimentarDB");
            else return RedirectToAction("Index");
        }

        public ActionResult ConfirmacaoSucessoAlimentarDB() {
            return View();
        }
    }
}
