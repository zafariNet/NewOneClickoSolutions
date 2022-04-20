using System;
using System.Collections.Generic;

namespace OneClickSolutions.Infrastructure.Metadata
{
    public class EntityMetadata
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<EntityViewMetadata> Views { get; set; }
        public Type ServiceType { get; set; }
    }
}