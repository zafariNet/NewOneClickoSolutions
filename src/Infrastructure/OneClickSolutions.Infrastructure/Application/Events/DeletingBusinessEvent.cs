using System;
using System.Collections.Generic;
using System.Linq;
using OneClickSolutions.Infrastructure.Eventing;

namespace OneClickSolutions.Infrastructure.Application
{
    public class DeletingBusinessEvent<TModel, TKey> : IBusinessEvent
        where TModel : MasterModel<TKey> where TKey : IEquatable<TKey>
    {
        public DeletingBusinessEvent(IEnumerable<TModel> models)
        {
            Models = models.ToList();
        }

        public IReadOnlyList<TModel> Models { get; }
    }
}