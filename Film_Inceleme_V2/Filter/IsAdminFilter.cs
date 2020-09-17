using Film_Inceleme_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Film_Inceleme_V2.Filter
{
    public class IsAdminFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.ActionDescriptor.IsDefined(typeof(IgnoreFilter), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(IgnoreFilter), true))
                return;

            bool isAuth = false;

            var kullanici = (Kullanici)filterContext.HttpContext.Session["Kullanici"];
            if (kullanici != null)
            {
                if (kullanici.Yetkilendirme.Yetki_ID == 1)
                {
                    isAuth = true;    
                }
            }

            if (!isAuth)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }
    }
}