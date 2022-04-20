using System;

namespace OneClickSolutions.Infrastructure.Exceptions
{
    [Serializable]
    public class DbException : Exception
    {
        public DbException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}