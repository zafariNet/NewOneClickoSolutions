using System;

namespace OneClickSolutions.Infrastructure.Timing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class DateTimeKindAttribute : Attribute
    {
        public DateTimeKind Kind { get; }

        public DateTimeKindAttribute(DateTimeKind kind)
        {
            Kind = kind;
        }
    }
}