using OneClickSolutions.Infrastructure.Domain;

namespace OneClickSolutions.Infrastructure.Numbering
{
    public class NumberedEntity : Entity
    {
        public string EntityName { get; set; }
        public long NextValue { get; set; }
    }
}