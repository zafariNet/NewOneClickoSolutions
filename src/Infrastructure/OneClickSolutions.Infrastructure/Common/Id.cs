using System;

namespace OneClickSolutions.Infrastructure.Common
{
    public class Id : Id<int>
    {
    }

    public class Id<TValue> where TValue : IEquatable<TValue>
    {
        public TValue Value { get; set; }
    }
}