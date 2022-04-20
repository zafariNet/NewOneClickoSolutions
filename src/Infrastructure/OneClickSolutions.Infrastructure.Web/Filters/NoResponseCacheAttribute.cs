namespace OneClickSolutions.Infrastructure.Web.Filters
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents an attribute that is used to mark an action method whose output will not be cached.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NoResponseCacheAttribute : ResponseCacheAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoResponseCacheAttribute"/> class.
        /// </summary>
        public NoResponseCacheAttribute()
        {
            NoStore = true;
            Location = ResponseCacheLocation.None;
        }
    }
}