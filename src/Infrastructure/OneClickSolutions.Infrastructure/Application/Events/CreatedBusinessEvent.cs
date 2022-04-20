using OneClickSolutions.Infrastructure.Eventing;

namespace OneClickSolutions.Infrastructure.Application
{
    public class CreatedBusinessEvent<TModel, TKey> : IBusinessEvent
        where TModel : MasterModel<TKey> where TKey : IEquatable<TKey>
    {
        public CreatedBusinessEvent(IEnumerable<TModel> models)
        {
            Models = models.ToList();
        }

        public IReadOnlyList<TModel> Models { get; }
    }
}