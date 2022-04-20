using System;

namespace OneClickSolutions.Infrastructure.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class SkipNormalizationAttribute : Attribute
    {
    }
}