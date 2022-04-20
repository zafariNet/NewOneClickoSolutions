using System;

namespace OneClickSolutions.Infrastructure.Exceptions
{
    [Serializable]
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message) : base(message)
        {
        }
    }
}