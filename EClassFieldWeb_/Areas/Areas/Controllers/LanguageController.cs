using EClassField.Core.Domain.Locazition;
using EClassField.Services.Catalog;
using EClassFieldWeb_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Areas.Areas.Controllers
{
    [AdminFilter]
    public class LanguageController : Controller
    {
        LanguageService languageservice = new LanguageService();
        // GET: Areas/Language
        public ActionResult Index(int Id = 0)
        {
            Language lan = new Language();
            if (Id > 0)
                lan = languageservice.GetById(d => d.Id == Id);


            var langues = languageservice.GetTable().ToList();
            ViewBag.languages = langues;
            return View(lan);
        }

        [HttpPost]

        public ActionResult Index(Language lang)
        {
            Language language = new Language();


            if (lang.Id >= 1)
            {
                language = languageservice.GetById(d => d.Id == lang.Id);

                UpdateModel(language);

                languageservice.Update();
            }
            else
            {
                UpdateModel(language);
                languageservice.Add(language);
            }
        
            return Redirect("/Panel/Language/Index/?Id=" + language.Id);
        }


        public ActionResult Delete(int id)
        {
            Language lan = new Language();
            lan = languageservice.GetById(d => d.Id == id);
            if (lan != null)
                languageservice.Delete(lan);

            return Redirect("/Panel/Language/Index/");
        }
    }
}