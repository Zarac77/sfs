using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Sfs.Models;
using System.Text;

namespace Sfs.Services
{
    public class ServicoLog : Servico
    {
        public static void Log(SfsContext context, string tipo, string texto) {
            LogReport logReport;
            if (context.LogReports.Any()) {
                logReport = context.LogReports.OrderByDescending(i => i.Descricao).First(l => l.Descricao.Contains(tipo));
            }
            else logReport = null;
            string log = System.DateTime.Now.ToString() + "\t" + texto + "<br/>";

            if (logReport != null && int.Parse(logReport.Descricao.Split('#')[1]) == GetWeekOfYear()) {
                logReport.Log += log;
            }
            else {
                CriarLogReport(context, tipo);
            }
            context.SaveChanges();
        }

        public static void CriarLogReport(SfsContext context, string tipo) {
            var lr = new LogReport { Descricao = tipo + " #" + GetWeekOfYear().ToString(), Id = Guid.NewGuid() };
            context.LogReports.Add(lr);
        }

        public static Stream GetLogStream(SfsContext context, Guid idLogReport) {
            var log = context.LogReports.Find(idLogReport).Log;
            log = log.Replace("<br/>", "\n");
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(log));
            return mStream;            
        }
    }
}