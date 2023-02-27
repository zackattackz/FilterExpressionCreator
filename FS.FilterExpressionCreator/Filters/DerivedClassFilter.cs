using System;

namespace FS.FilterExpressionCreator.Filters
{
    internal class DerivedClassFilter
    {
        public Type DerivedClassType { get; }

        public EntityFilter EntityFilter { get; }

        public DerivedClassFilter(Type derivedClassType, EntityFilter entityFilter)
        {
            DerivedClassType = derivedClassType ?? throw new ArgumentNullException(nameof(derivedClassType));
            EntityFilter = entityFilter ?? new EntityFilter();
        }
    }
}
