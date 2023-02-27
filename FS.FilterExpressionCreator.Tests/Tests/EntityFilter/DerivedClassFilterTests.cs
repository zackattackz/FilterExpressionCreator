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
        [DataTestMethod]
        [FilterFuncDataSource(nameof(GetEntityFilterFunctions), typeof(TestModel<string>))]
        public void WhenDerivedClassAFilterIsApplied_ThenDerivedClassAIsFilteredFor(EntityFilterFunc<TestModel<string>> filterFunc)
        {
            var derivedClassFilter = new EntityFilter<TestModelDerivedClassA<string>>()
                .Replace(x => x.SubValueA, "=SubValueA");

            var superclassFilter = new EntityFilter<TestModel<string>>()
                .Replace(x => x.ValueA, "=ValueA")
                .AddDerivedClassFilter(derivedClassFilter);

            var testItems = new List<TestModel<string>>()
            {
                new TestModelDerivedClassA<string> { ValueA = "ValueA", SubValueA = "SubValueA" },
                new TestModelDerivedClassA<string> { ValueA = "ValueB", SubValueA = "SubValueA"},
                new TestModelDerivedClassA<string> { ValueA = "ValueA", SubValueA = "SubValueB"},
                new TestModel<string> { ValueA = "ValueA" },
                new TestModel<string> { ValueA = "ValueB" },
                new TestModelDerivedClassB<string> { ValueA = "ValueA", SubValueB = "SubValueB"},
            };

            var filteredEntities = filterFunc(testItems, superclassFilter);

            filteredEntities.Should().BeEquivalentTo(new[] { testItems[0] });
        }
    }
}
