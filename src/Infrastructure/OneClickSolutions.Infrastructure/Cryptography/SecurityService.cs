using System;
using System.Security.Cryptography;
using System.Text;
using OneClickSolutions.Infrastructure.Dependency;

namespace OneClickSolutions.Infrastructure.Cryptography
{
    public interface ISecurityService : ISingletonDependency
    {
        Guid NewSecureGuid();
        string ComputeSha256Hash(string input);
    }

    internal sealed class SecurityService : ISecurityService
    {
        private readonly RandomNumberGenerator _rand = RandomNumberGenerator.Create();

        public Guid NewSecureGuid()
        {
            var bytes = new byte[16];
            _rand.GetBytes(bytes);
            return new Guid(bytes);
        }

        public string ComputeSha256Hash(string input)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
    }
}