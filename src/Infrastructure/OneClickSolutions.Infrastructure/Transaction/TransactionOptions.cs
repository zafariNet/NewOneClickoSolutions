using System;
using System.Data;

namespace OneClickSolutions.Infrastructure.Transaction
{
    public sealed class TransactionOptions
    {
        public TimeSpan? Timeout { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }
    }
}