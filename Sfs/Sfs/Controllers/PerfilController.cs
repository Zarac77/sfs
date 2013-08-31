using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

using Sfs.Models;

namespace Sfs.Controllers
{   
    public class PerfilController : CustomController
    {
        //
        // GET: /Perfil/

        public ActionResult Index()
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            return View(Context.Perfis.Include(perfil => perfil.Pessoas).ToList());
        }

        //
        // GET: /Perfil/Details/5

        public ViewResult Details(System.Guid id)
        {
            Perfil perfil = Context.Perfis.Single(x => x.Id == id);
            return View(perfil);
        }

        //
        // GET: /Perfil/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Perfil/Create

        [HttpPost]
        public ActionResult Create(Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                perfil.Id = Guid.NewGuid();
                Context.Perfis.Add(perfil);
                Context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(perfil);
        }
        
        //
        // GET: /Perfil/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
            Perfil perfil = Context.Perfis.Single(x => x.Id == id);
            return View(perfil);
        }

        //
        // POST: /Perfil/Edit/5

        [HttpPost]
        public ActionResult Edit(Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                Context.Entry(perfil).State = EntityState.Modified;
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(perfil);
        }

        //
        // GET: /Perfil/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            Perfil perfil = Context.Perfis.Single(x => x.Id == id);
            return View(perfil);
        }

        //
        // POST: /Perfil/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            Perfil perfil = Context.Perfis.Single(x => x.Id == id);
            Context.Perfis.Remove(perfil);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}