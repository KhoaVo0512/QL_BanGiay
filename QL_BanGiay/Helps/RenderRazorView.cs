﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using QL_BanGiay.Data;
using QL_BanGiay.Areas.Admin.Models;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QL_BanGiay.Helps
{
    public class RenderRazorView
    {
        private readonly QlyBanGiayContext _context;
        public RenderRazorView(QlyBanGiayContext context)
        {
            _context = context;
        }
        public static string RenderRazorViewToString(Controller controller, string viewName, object? model = null, object? page=null, string? search= "")
        {
            controller.ViewBag.Pager = page;
            controller.ViewBag.SearchText = search;
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);
               
                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.HttpContext.Request.GetTypedHeaders().Referer == null || filterContext.HttpContext.Request.GetTypedHeaders().Host.Host.ToString() != filterContext.HttpContext.Request.GetTypedHeaders().Referer.Host.ToString())
                {
                    filterContext.HttpContext.Response.Redirect("/admin");
                }
            }
        }
    }
}
