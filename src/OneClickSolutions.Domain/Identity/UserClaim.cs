namespace OneClickSolutions.Domain.Identity
{
    public class UserClaim : Claim
    {
        public User User { get; set; }
        public long UserId { get; set; }
    }
}