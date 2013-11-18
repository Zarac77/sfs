using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using Sfs.Models;
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
            var stream = source.InputStream;
            byte[] buf = new byte[stream.Length];
            if (stream.Length < int.MaxValue)
            {
                stream.Read(buf, 0, (int)stream.Length);
            }
            var textoFile = Encoding.UTF8.GetString(buf);
            StringReader reader = new StringReader(textoFile);
            string linha = reader.ReadLine();
            List<Pessoa> pessoas = new List<Pessoa>();
            while (linha != null)
            {
                string[] dados = linha.Split(';');
                Pessoa p = new Pessoa
                {
                    Id = Guid.NewGuid(),
                    Nome = dados[0],
                    Matricula = dados[1],
                    Turma = dados[3],
                    Email = string.IsNullOrEmpty(dados[5]) ? Guid.NewGuid() + "@escolasesc.g12.br" : dados[5],
                    Perfis = new List<Perfil> { Context.Perfis.Single(pf => pf.Id == Perfil.GUID_PERFIL_ALUNO) }
                };
                p.Senha = ServicoControleAcesso.HashSenha(p.Email, "123456"); //Linha em separado para pegar e-mail.
                pessoas.Add(p);
                linha = reader.ReadLine();
            }
            pessoas.ForEach(p => Context.Inboxes.Add(new Inbox { Id = Guid.NewGuid(), IdPessoa = p.Id, Pessoa = p}));
            pessoas.ForEach(p => Context.Pessoas.Add(p));
            Context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DroparBanco()
        {
            /*Context.Database.Delete();
            Context.Database.Initialize(true);*/
            return RedirectToAction("Index");
        }
    }
}
