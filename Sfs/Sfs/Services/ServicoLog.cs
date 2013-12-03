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
        /// <summary>
        /// Adiciona uma entrada a uma coleção de logs (LogReport).
        /// </summary>
        /// <param name="context">O contexto do Banco de Dados.</param>
        /// <param name="tipo">Nome da coleção (caso não exista, será criada).</param>
        /// <param name="texto">Texto a ser adicionado à coleção.</param>
        public static void Log(SfsContext context, string tipo, string texto) {
            LogReport logReport;
            if (context.LogReports.Any()) {
                logReport = context.LogReports.OrderByDescending(i => i.Descricao).First(l => l.Descricao.Contains(tipo));
            }
            else logReport = null;

            //Garante um log por semana.
            if (logReport != null && int.Parse(logReport.Descricao.Split('#')[1]) == GetWeekOfYear()) {
                logReport.Log += texto;
            }
            else {
                CriarLogReport(context, tipo);
                Log(context, tipo, texto);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Retorna uma linha de texto pronta para o log.
        /// </summary>
        /// <param name="texto">Texto a ser formatado.</param>
        /// <returns></returns>
        public static string FormatarLog(string texto) {
            string log = DateTime.Now.ToString() + "\t" + texto + "<br/>";
            return log;
        }

        private static void CriarLogReport(SfsContext context, string tipo) {
            var lr = new LogReport { Descricao = tipo + " #" + GetWeekOfYear().ToString(), Id = Guid.NewGuid() };
            context.LogReports.Add(lr);
            context.SaveChanges();
        }

        public static Stream GetLogStream(SfsContext context, Guid idLogReport) {
            var log = context.LogReports.Find(idLogReport).Log;
            log = log.Replace("<br/>", "\r\n");
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(log));
            return mStream;            
        }
    }
}