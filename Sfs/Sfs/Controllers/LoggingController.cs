using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sfs.Services;

namespace Sfs.Controllers
{
    public class LoggingController : CustomController
    {
        //
        // GET: /Logging/

        public ActionResult Index()
        {
            var listaLogReports = Context.LogReports.ToList();
            return View(listaLogReports);
        }

        public string Ver(Guid id) {
            return Context.LogReports.Find(id).Log;
        }

        public FileStreamResult Download(Guid id) {
            var str = ServicoLog.GetLogStream(Context, id);
            var fileName = Context.LogReports.Find(id).Descricao;
            //var buffer = new byte[str.Length];
            //str.Read(buffer, 0, (int)str.Length);
            return new FileStreamResult(str, "text/plain") { FileDownloadName = fileName + ".txt" };
        }
    }
}
