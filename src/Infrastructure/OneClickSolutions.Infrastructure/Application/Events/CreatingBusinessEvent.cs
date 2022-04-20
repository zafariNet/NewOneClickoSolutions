using System;
using System.Collections.Generic;
using System.Linq;
using OneClickSolutions.Infrastructure.Eventing;

namespace OneClickSolutions.Infrastructure.Application
{
    public class CreatingBusinessEvent<TModel, TKey> : IBusinessEvent
        where TModel : MasterModel<TKey> where TKey : IEquatable<TKey>
    {
        public CreatingBusinessEvent(IEnumerable<TModel> models)
        {
            Models = models.ToList();
        }

        public IReadOnlyList<TModel> Models { get; }
    }
}