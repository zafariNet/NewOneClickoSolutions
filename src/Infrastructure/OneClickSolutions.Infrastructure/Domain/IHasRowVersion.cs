namespace OneClickSolutions.Infrastructure.Domain
{
    public interface IHasRowVersion
    {
        byte[] Version { get; set; }
    }
}