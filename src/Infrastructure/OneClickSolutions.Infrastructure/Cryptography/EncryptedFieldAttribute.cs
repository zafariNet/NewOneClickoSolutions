using System;

namespace OneClickSolutions.Infrastructure.Cryptography
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EncryptedFieldAttribute : Attribute
    { }
}