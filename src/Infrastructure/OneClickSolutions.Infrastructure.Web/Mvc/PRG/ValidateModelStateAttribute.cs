using System;
using OneClickSolutions.Infrastructure.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OneClickSolutions.Infrastructure.Web.Mvc.PRG
{
    /// <summary>
    /// An ActionFilter for automatically validating ModelState before a controller action is executed.
    /// Performs a Redirect if ModelState is invalid. Assumes the <see cref="ImportModelStateAttribute"/> is used on the GET action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateModelStateAttribute : ModelStateTransfer
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    ProcessAjax(filterContext);
                }
                else
                {
                    ProcessNormal(filterContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        protected virtual void ProcessNormal(ActionExecutingContext filterContext)
        {
            // Export ModelState to TempData so it's available on next request
            ExportModelState(filterContext);

            // Redirect back to GET action
            filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
        }

        protected virtual void ProcessAjax(ActionExecutingContext filterContext)
        {
            filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
            filterContext.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}