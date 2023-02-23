using System;

namespace FS.FilterExpressionCreator.Filters
{
    internal class SubclassFilter
    {
        public Type SubclassType { get; }

        public EntityFilter EntityFilter { get; }

        public bool IsInclusive { get; set; }

        public SubclassFilter(Type subclassType, EntityFilter entityFilter, bool isInclusive)
        {
            SubclassType = subclassType ?? throw new ArgumentNullException(nameof(subclassType));
            EntityFilter = entityFilter ?? new EntityFilter();
            IsInclusive = isInclusive;
        }
    }
}
