using System.Reflection;
using OneClickSolutions.Infrastructure.Collections;
using OneClickSolutions.Infrastructure.Reflection;
using OneClickSolutions.Infrastructure.Validation;

namespace OneClickSolutions.Infrastructure.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool ValidationIgnored(this MethodInfo method)
        {
            var parameters = method.GetParameters();
            return !method.IsPublic || parameters.IsNullOrEmpty() || IsValidationSkipped(method);
        }

        private static bool IsValidationSkipped(MemberInfo method)
        {
            if (method.IsDefined(typeof(EnableValidationAttribute), true))
            {
                return false;
            }

            return method
                       .GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<SkipValidationAttribute>() != null;
        }
    }
}