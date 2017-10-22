using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentMex.Logic;
using DentMex.Domain;

namespace DentMex.WebUI.Controllers
{
    public class HomeController : AbstractController
    {

        public HomeController(IAccountService AccountService, IVisitService VisitService) : base(AccountService, VisitService) { }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

    }
}