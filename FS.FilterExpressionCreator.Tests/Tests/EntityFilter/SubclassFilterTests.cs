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
    public class SubclassFilterTests : TestBase
    {
        [DataTestMethod]
        [FilterFuncDataSource(nameof(GetEntityFilterFunctions), typeof(TestModel<string>))]
        public void WhenSubclassAFilterIsApplied_ThenSubclassBIsFiltered(EntityFilterFunc<TestModel<string>> filterFunc)
        {
            var subclassFilter = new EntityFilter<TestModelSubclassA<string>>()
                .Replace(x => x.SubValueA, "=SubValueA");

            var superclassFilter = new EntityFilter<TestModel<string>>()
                .Replace(x => x.ValueA, "=ValueA")
                .AddSubclassFilter(subclassFilter);

            var testItems = new List<TestModel<string>>()
            {
                new TestModelSubclassA<string> { ValueA = "ValueA", SubValueA = "SubValueA" },
                new TestModelSubclassA<string> { ValueA = "ValueB", SubValueA = "SubValueA"},
                new TestModelSubclassA<string> { ValueA = "ValueA", SubValueA = "SubValueB"},
                new TestModel<string> { ValueA = "ValueA" },
                new TestModel<string> { ValueA = "ValueB" },
                new TestModelSubclassB<string> { ValueA = "ValueA", SubValueB = "SubValueB"},
            };

            var filteredEntities = filterFunc(testItems, superclassFilter);

            filteredEntities.Should().BeEquivalentTo(new[] { testItems[0] });
        }
    }
}
