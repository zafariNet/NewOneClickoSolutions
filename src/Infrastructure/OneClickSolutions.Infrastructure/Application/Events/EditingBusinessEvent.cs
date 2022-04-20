﻿using System;
using System.Collections.Generic;
using System.Linq;
using OneClickSolutions.Infrastructure.Eventing;

namespace OneClickSolutions.Infrastructure.Application
{
    public class EditingBusinessEvent<TModel, TKey> : IBusinessEvent
        where TModel : MasterModel<TKey> where TKey : IEquatable<TKey>
    {
        public EditingBusinessEvent(IEnumerable<ModifiedModel<TModel>> models)
        {
            Models = models.ToList();
        }

        public IReadOnlyList<ModifiedModel<TModel>> Models { get; }
    }
}