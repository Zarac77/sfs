using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sfs.Models;

namespace Sfs.Controllers
{
    public class AlunoController : CustomController
    {
        //
        // GET: /Aluno/Details/5

        public ActionResult VisualizarAtividades() {
            var atividades = Context.Atividades.Where(a => a.DataHoraInicio >= DateTime.Today).ToList();            
            return View(atividades);
        }

        [HttpPost]
        public ActionResult VisualizarAtividades(Guid id, bool desistir) {
            /*
            var inscricao = Context.Atividades.Single((a => a.Id == id));
            if (desistir) {
                var aluno = inscricao.Inscricoes.First().Inscritos.Single(a => a.Matricula == "2011.0004");
                inscricao.Inscricoes.First().Inscritos.Remove(aluno);
            }
            else {
                
                inscricao.Inscricoes[0].Inscritos.Add(new Aluno { Id = Guid.NewGuid(), Matricula = "2011.0004", Nome = "Westerbly Snaydley" });
            }
            */
            var atividades = Context.Atividades.Where(a => a.DataHoraInicio >= DateTime.Today).ToList();
            Context.SaveChanges();
            return View(atividades);
        }

        /*public ActionResult Details(Guid id)
        {
            Aluno aluno = (Aluno)Context.Pessoas.Find(id);
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
        }

        //
        // GET: /Aluno/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Aluno/Create

        [HttpPost]
        public ActionResult Create(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                aluno.Id = Guid.NewGuid();
                Context.Pessoas.Add(aluno);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aluno);
        }

        //
        // GET: /Aluno/Edit/5

        public ActionResult Edit(Guid id)
        {
            Aluno aluno = (Aluno)Context.Pessoas.Find(id);
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
        }

        //
        // POST: /Aluno/Edit/5

        [HttpPost]
        public ActionResult Edit(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                Context.Entry(aluno).State = EntityState.Modified;
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aluno);
        }

        //
        // GET: /Aluno/Delete/5

        public ActionResult Delete(Guid id)
        {
            Aluno aluno = (Aluno)Context.Pessoas.Find(id);
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
        }

        //
        // POST: /Aluno/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Aluno aluno = (Aluno)Context.Pessoas.Find(id);
            Context.Pessoas.Remove(aluno);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }*/
    }
}