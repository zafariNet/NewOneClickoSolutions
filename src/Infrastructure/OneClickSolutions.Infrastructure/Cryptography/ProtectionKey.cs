using OneClickSolutions.Infrastructure.Domain;

namespace OneClickSolutions.Infrastructure.Cryptography
{
    public class ProtectionKey : Entity<long>
    {
        public string FriendlyName { get; set; }
        public string XmlValue { get; set; }
    }
}