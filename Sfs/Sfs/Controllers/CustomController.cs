using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

using Sfs.Models;

namespace Sfs.Controllers
{
    public class CustomController : Controller
    {
        private SfsContext context;
        public SfsContext Context
        {
            get
            {
                if (context == null)
                    context = new SfsContext();

                return context;
            }
        }

        public Pessoa PessoaLogada
        {
            get { return GetPessoaLogada(); }
        }

        private Pessoa GetPessoaLogada()
        {
            IPrincipal principal;

            if (HttpContext == null)
                return null;
            else
            {
                principal = HttpContext.User;

                if (principal == null)
                    return null;
                else
                    return Context.Pessoas.AsNoTracking().SingleOrDefault(u => u.Email == principal.Identity.Name);
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.PessoaLogada = PessoaLogada;
            if (PessoaLogada != null)
            {
                var inbox = Context.Inboxes.Single(i => i.IdPessoa == PessoaLogada.Id);
                ViewBag.Inbox = inbox;
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}