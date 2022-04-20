﻿using System;
using OneClickSolutions.Infrastructure.Dependency;

namespace OneClickSolutions.Infrastructure.Timing
{
    public interface ISystemClock : ISingletonDependency
    {
        /// <summary>
        /// Retrieves the current system time in UTC.
        /// </summary>
        DateTime Now { get; }
        DateTime Normalize(DateTime dateTime);
    }

    internal sealed class SystemClock : ISystemClock
    {
        //<inherit>
        public DateTime Now => SystemTime.Now();

        public DateTime Normalize(DateTime dateTime)
        {
            return SystemTime.Normalize(dateTime);
        }
    }
}