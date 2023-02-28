using FluentAssertions;
using FS.FilterExpressionCreator.Extensions;
using FS.FilterExpressionCreator.Filters;
using FS.FilterExpressionCreator.Tests.Attributes;
using FS.FilterExpressionCreator.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FS.FilterExpressionCreator.Tests.Tests.EntityFilter
{
    [TestClass, ExcludeFromCodeCoverage]
    public class DerivedClassFilterTests : TestBase
    {
        private List<TestModel<string>> _testItems = new List<TestModel<string>>()
        {
            new TestModelDerivedClassA<string> { ValueA = "ValueA", SubValueA = "SubValueA" },
            new TestModelDerivedClassA<string> { ValueA = "ValueB", SubValueA = "SubValueA"},
            new TestModelDerivedClassA<string> { ValueA = "ValueA", SubValueA = "SubValueB"},
            new TestModel<string> { ValueA = "ValueA" },
            new TestModel<string> { ValueA = "ValueB" },
            new TestModelDerivedClassB<string> { ValueA = "ValueA", SubValueB = "SubValueB"},
        };

        [DataTestMethod]
        [FilterFuncDataSource(nameof(GetEntityFilterFunctions), typeof(TestModel<string>))]
        public void WhenEmptyDerivedClassAFilterIsApplied_ThenAnyClassAInstancesAreFilteredFor(EntityFilterFunc<TestModel<string>> filterFunc)
        {
            var derivedClassFilterA = new EntityFilter<TestModelDerivedClassA<string>>();

            var baseClassFilter = new EntityFilter<TestModel<string>>()
                .AddDerivedClassFilter(derivedClassFilterA);

            var filteredEntities = filterFunc(_testItems, baseClassFilter);

            filteredEntities.Should().BeEquivalentTo(new[] { _testItems[0], _testItems[1], _testItems[2] });
        }

        [DataTestMethod]
        [FilterFuncDataSource(nameof(GetEntityFilterFunctions), typeof(TestModel<string>))]
        public void WhenDerivedClassAFilterIsApplied_ThenDerivedClassAIsFilteredFor(EntityFilterFunc<TestModel<string>> filterFunc)
        {
            var derivedClassFilter = new EntityFilter<TestModelDerivedClassA<string>>()
                .Replace(x => x.SubValueA, "=SubValueA");

            var baseClassFilter = new EntityFilter<TestModel<string>>()
                .AddDerivedClassFilter(derivedClassFilter);

            var filteredEntities = filterFunc(_testItems, baseClassFilter);

            filteredEntities.Should().BeEquivalentTo(new[] { _testItems[0], _testItems[1] });
        }

        [DataTestMethod]
        [FilterFuncDataSource(nameof(GetEntityFilterFunctions), typeof(TestModel<string>))]
        public void WhenDerivedClassAandBFiltersAreApplied_ThenDerivedClassAandBAreFilteredFor(EntityFilterFunc<TestModel<string>> filterFunc)
        {
            var derivedClassFilterA = new EntityFilter<TestModelDerivedClassA<string>>()
                .Add(x => x.SubValueA, "=SubValueA");

            var derivedClassFilterB = new EntityFilter<TestModelDerivedClassB<string>>()
                .Replace(x => x.SubValueB, "=SubValueB");

            var baseClassFilter = new EntityFilter<TestModel<string>>()
                .AddDerivedClassFilter(derivedClassFilterA)
                .AddDerivedClassFilter(derivedClassFilterB);

            var filteredEntities = filterFunc(_testItems, baseClassFilter);

            filteredEntities.Should().BeEquivalentTo(new[] { _testItems[0], _testItems[1], _testItems[5] });
        }

        [DataTestMethod]
        [FilterFuncDataSource(nameof(GetEntityFilterFunctions), typeof(TestModel<string>))]
        public void WhenDerivedClassFilterAIsReplaced_ThenTheNewFilterIsUsed(EntityFilterFunc<TestModel<string>> filterFunc)
        {
            var derivedClassFilterA = new EntityFilter<TestModelDerivedClassA<string>>()
                .Add(x => x.SubValueA, "=SubValueA");

            var derivedClassFilterANew = new EntityFilter<TestModelDerivedClassA<string>>()
                .Add(x => x.SubValueA, "=SubValueB");

            var baseClassFilter = new EntityFilter<TestModel<string>>()
                .AddDerivedClassFilter(derivedClassFilterA)
                .ReplaceDerivedClassFilter(derivedClassFilterANew);

            var filteredEntities = filterFunc(_testItems, baseClassFilter);

            filteredEntities.Should().BeEquivalentTo(new[] { _testItems[2] });
        }

        [TestMethod]
        public void WhenConflictOnAddDerivedClassFilter_ThenArgumentExceptionIsThrown()
        {
            var derivedClassFilter = new EntityFilter<TestModelDerivedClassA<string>>()
                .Replace(x => x.ValueA, "=A");

            ((Action)(() => new EntityFilter<TestModel<string>>()
                .Add(x => x.ValueA, "=B")
                .AddDerivedClassFilter(derivedClassFilter)))
                .Should()
                .Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenConflictOnAdd_ThenArgumentExceptionIsThrown()
        {
            var derivedClassFilter = new EntityFilter<TestModelDerivedClassA<string>>()
                .Replace(x => x.ValueA, "=A");

            ((Action)(() => new EntityFilter<TestModel<string>>()
                .AddDerivedClassFilter(derivedClassFilter)
                .Add(x => x.ValueA, "=B")))
                .Should()
                .Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenNonDerivedClassFilterIsAdded_ThenArgumentExceptionIsThrown()
        {
            var nonDerivedClassFilter = new EntityFilter<TestModel<string>>();

            ((Action)(() => new EntityFilter<TestModel<string>>()
                .AddDerivedClassFilter(nonDerivedClassFilter)))
                .Should()
                .Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenNonDerivedClassFilterIsReplaced_ThenArgumentExceptionIsThrown()
        {
            var nonDerivedClassFilter = new EntityFilter<TestModel<string>>();

            ((Action)(() => new EntityFilter<TestModel<string>>()
                .ReplaceDerivedClassFilter(nonDerivedClassFilter)))
                .Should()
                .Throw<ArgumentException>();
        }
    }
}
