using Parallax.Helpers;
using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace Parallax.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly ParallaxContext context;

        public TestimonialController() 
        { 

        context = new ParallaxContext();

        }
        
        public ActionResult Testimonial()
        {
            TBLPAGE pageModel = context.TBLPAGEs.FirstOrDefault();            
            TimeViewModel timeModel = TimeModelHelper.GetTimeModel(context.TBLPAGEs.FirstOrDefault());

            IndexViewModel testimonialViewModel = new IndexViewModel()
            {
                PageModel = pageModel,
                TimeModel = timeModel
            };
            return View(testimonialViewModel);
        }
    }
}