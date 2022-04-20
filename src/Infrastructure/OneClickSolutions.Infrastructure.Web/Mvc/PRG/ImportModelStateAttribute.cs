using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OneClickSolutions.Infrastructure.Web.Mvc.PRG
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ImportModelStateAttribute : ModelStateTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!(filterContext.Controller is Controller controller) || filterContext.ModelState == null) return;

            if (filterContext.Result is ViewResult)
            {
                ImportModelState(filterContext);
            }
            else
            {
                RemoveModelState(filterContext);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}