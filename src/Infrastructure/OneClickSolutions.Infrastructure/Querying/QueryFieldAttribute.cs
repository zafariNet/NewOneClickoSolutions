using System;

namespace OneClickSolutions.Infrastructure.Querying
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryFieldAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Sorting { get; set; }
        public bool Filtering { get; set; }
    }
}