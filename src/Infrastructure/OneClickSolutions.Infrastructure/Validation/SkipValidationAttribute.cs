using System;

namespace OneClickSolutions.Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class )]
    public sealed class SkipValidationAttribute : Attribute
    {
    }
}