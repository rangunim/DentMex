using DentMex.Domain;
using DentMex.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace DentMex.WebUI.Controllers
{
    public abstract class AbstractController : Controller
    {
        protected IAccountService AccountService;
        protected IVisitService VisitService;
        //public AbstractController() { }

        public AbstractController(IAccountService AccountService, IVisitService VisitService)
        {
            this.AccountService = AccountService;
            this.VisitService = VisitService;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if(HttpContext.User != null)
            {
                UserRole role = AccountService.GetUserRole(GetIdFromIdentity(HttpContext.User.Identity.Name));
                if(role != null)
                {
                    HttpContext.User = new GenericPrincipal(HttpContext.User.Identity, new string [] { role.RoleName });
                }
            }
        }

        protected string GetIdFromIdentity(string identity)
        {
            if(HttpContext.User != null && identity == null || identity.Equals(""))
                return null;
            int start = identity.IndexOf('(');
            int end = identity.IndexOf(')');
            string result = identity.Substring(start + 1, end - start - 1);
            return result;
        }

    }
}