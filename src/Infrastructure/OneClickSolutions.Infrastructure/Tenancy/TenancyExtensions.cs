using System.Linq;
using System.Reflection;

namespace OneClickSolutions.Infrastructure.Tenancy
{
    public static class TenancyExtensions
    {
        private static TenancySides FindTenancySides(this object instance)
        {
            var attribute= instance.GetType().GetTypeInfo()
                .GetCustomAttributes(typeof(TenancySideAttribute), true)
                .Cast<TenancySideAttribute>()
                .FirstOrDefault();

            return attribute?.Side ?? TenancySides.None;
        }
    }
}