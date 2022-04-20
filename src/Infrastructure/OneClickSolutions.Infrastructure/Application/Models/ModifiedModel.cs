namespace OneClickSolutions.Infrastructure.Application
{
    public class ModifiedModel<TValue>
    {
        public TValue NewValue { get; set; }
        public TValue OriginalValue { get; set; }
    }
}