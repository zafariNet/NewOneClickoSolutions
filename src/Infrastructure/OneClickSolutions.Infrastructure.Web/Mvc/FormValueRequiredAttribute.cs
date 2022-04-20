using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OneClickSolutions.Infrastructure.Web.Mvc
{
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        private readonly string[] _submitButtonNames;
        private readonly FormValueRequirement _requirement;
        private readonly FormValueRequirementRule _rule;
        private readonly bool _inverse;

        public FormValueRequiredAttribute(params string[] submitButtonNames) :
            this(FormValueRequirement.Equal, FormValueRequirementRule.MatchAny, false, submitButtonNames)
        {
        }

        public FormValueRequiredAttribute(FormValueRequirement requirement, params string[] submitButtonNames)
            : this(requirement, FormValueRequirementRule.MatchAny, false, submitButtonNames)
        {
        }

        public FormValueRequiredAttribute(FormValueRequirementRule rule, params string[] submitButtonNames)
            : this(FormValueRequirement.Equal, rule, false, submitButtonNames)
        {
        }

        public FormValueRequiredAttribute(FormValueRequirement requirement, FormValueRequirementRule rule, params string[] submitButtonNames)
            : this(requirement, rule, false, submitButtonNames)
        {
        }

        protected FormValueRequiredAttribute(
            FormValueRequirement requirement,
            FormValueRequirementRule rule,
            bool inverse,
            params string[] submitButtonNames)
        {
            // at least one submit button should be found (or being absent if 'inverse')
            _submitButtonNames = submitButtonNames;
            _requirement = requirement;
            _rule = rule;
            _inverse = inverse;
        }

        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            return IsValidForRequest(routeContext);
        }


        protected virtual bool IsValidForRequest(RouteContext routeContext)
        {
            var logger = routeContext.HttpContext.RequestServices.GetRequiredService<ILogger<FormValueRequiredAttribute>>();
            var form = routeContext.HttpContext.Request.Form;
            try
            {
                bool isMatch = false;
                if (_rule == FormValueRequirementRule.MatchAny)
                {
                    isMatch = _submitButtonNames.Any(x => IsMatch(form, x));
                }
                else
                {
                    isMatch = _submitButtonNames.All(x => IsMatch(form, x));
                }
                return isMatch;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return false;
        }

        private bool IsMatch(IFormCollection form, string key)
        {
            string value = "";

            if (_requirement == FormValueRequirement.Equal)
            {
                // do not iterate because "Invalid request" exception can be thrown
                value = form[key];
            }
            else
            {
                var firstMatch = form.Keys.FirstOrDefault(x => x.StartsWith(key, StringComparison.InvariantCultureIgnoreCase));
                if (firstMatch != null)
                {
                    value = form[firstMatch];
                }
            }

            if (_inverse)
            {
                return string.IsNullOrEmpty(value);
            }

            return !string.IsNullOrEmpty(value);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class FormValueAbsentAttribute : FormValueRequiredAttribute
    {
        public FormValueAbsentAttribute(params string[] submitButtonNames) :
            base(FormValueRequirement.Equal, FormValueRequirementRule.MatchAny, true, submitButtonNames)
        {
        }

        public FormValueAbsentAttribute(FormValueRequirement requirement, params string[] submitButtonNames)
            : base(requirement, FormValueRequirementRule.MatchAny, true, submitButtonNames)
        {
        }

        public FormValueAbsentAttribute(FormValueRequirementRule rule, params string[] submitButtonNames)
            : base(FormValueRequirement.Equal, rule, true, submitButtonNames)
        {
        }

        public FormValueAbsentAttribute(FormValueRequirement requirement, FormValueRequirementRule rule, params string[] submitButtonNames)
            : base(requirement, rule, true, submitButtonNames)
        {
        }
    }

    public enum FormValueRequirement
    {
        Equal,
        StartsWith
    }

    public enum FormValueRequirementRule
    {
        MatchAny,
        MatchAll
    }
}