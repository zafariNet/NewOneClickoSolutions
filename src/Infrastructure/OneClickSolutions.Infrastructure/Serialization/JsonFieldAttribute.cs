using System;

namespace OneClickSolutions.Infrastructure.Serialization
{
    /// <summary>
    /// Marks the property to be serialized into database as JSON.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class JsonFieldAttribute : Attribute {}
}