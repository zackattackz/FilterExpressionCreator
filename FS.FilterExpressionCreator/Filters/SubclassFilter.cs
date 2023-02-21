using System;

namespace FS.FilterExpressionCreator.Filters
{
    internal class SubclassFilter
    {
        public Type SubclassType { get; }

        public EntityFilter EntityFilter { get; }

        public SubclassFilter(Type subclassType, EntityFilter entityFilter)
        {
            SubclassType = subclassType ?? throw new ArgumentNullException(nameof(subclassType));
            EntityFilter = entityFilter ?? new EntityFilter();
        }
    }
}
