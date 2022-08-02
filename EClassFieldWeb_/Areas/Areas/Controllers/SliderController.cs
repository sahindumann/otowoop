using EClassField.Core.Domain.Galerry;
using EClassField.Data;
using EClassFieldWeb_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Areas.Areas.Controllers
{
    [AdminFilter]
    public class SliderController : Controller
    {
        // GET: Areas/Slider
        public ActionResult Index(int sliderId = -1)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                var slider = ctx.Set<Slider>().ToList();
                ViewBag.Sliders = slider;
                if (sliderId > 0)
                {
                    var s = slider.Find(d => d.Id == sliderId);
                    ViewBag.Slider = s;
                    return View(s);
       }         
         

            }
            return View(new Slider());

        }

        [HttpPost]
        public ActionResult Index(Slider slider)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                if (slider.Id >= 1)

                {
                    var sldr = ctx.Set<Slider>().FirstOrDefault(d => d.Id == slider.Id);
                    if (sldr != null)
                    {
                        sldr.Image = slider.Image;
                        sldr.Video = slider.Video;
                        sldr.Baslik = slider.Baslik;
                        sldr.Aciklama = slider.Aciklama;
                        sldr.IsAktif = slider.IsAktif;
                        ctx.SaveChanges();
                    }
                }
                else
                {
                    var sldr = new Slider();
                    sldr.Image = slider.Image;
                    sldr.Video = slider.Video;
                    sldr.Baslik = slider.Baslik;
                    sldr.Aciklama = slider.Aciklama;
                    sldr.IsAktif = slider.IsAktif;
                    ctx.Set<Slider>().Add(sldr);
                  
                    ctx.SaveChanges();
                }
                Cache.Slider = ctx.Set<Slider>().FirstOrDefault(d => d.IsAktif);

                return RedirectToAction("/Index");
            }
            return View();
        }
    }
}