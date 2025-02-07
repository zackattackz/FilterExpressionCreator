﻿using FluentAssertions;
using FS.FilterExpressionCreator.Extensions;
using FS.FilterExpressionCreator.Filters;
using FS.FilterExpressionCreator.Tests.Attributes;
using FS.FilterExpressionCreator.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FS.FilterExpressionCreator.Tests.Tests.EntityFilter
{
    [TestClass, ExcludeFromCodeCoverage]
    public class CastFilterTests : TestBase
    {
        [TestMethod]
        public void WhenEntityFilterIsCast_ThenPropertiesNotExistsOrAreNotAssignableAreRemoved()
        {
            var dtoFilterSrc = new EntityFilter<ModelDto>()
                .Add(x => x.Name, "A")
                .Add(x => x.Age, 20)
                .Add(x => x.Created, DateTime.UtcNow);

            var modelFilterSrc = new EntityFilter<Model>()
                .Add(x => x.Name, "A")
                .Add(x => x.Age, 20)
                .Add(x => x.Created, DateTime.UtcNow);

            var modelFilter = dtoFilterSrc.Cast<Model>();
            var dtoFilter = modelFilterSrc.Cast<ModelDto>();

            modelFilter.PropertyFilters.Should().Contain(x => x.PropertyName == "Name");
            modelFilter.PropertyFilters.Should().NotContain(x => x.PropertyName == "Age");
            modelFilter.PropertyFilters.Should().NotContain(x => x.PropertyName == "Created");

            dtoFilter.PropertyFilters.Should().Contain(x => x.PropertyName == "Name");
            dtoFilter.PropertyFilters.Should().NotContain(x => x.PropertyName == "Age");
            dtoFilter.PropertyFilters.Should().Contain(x => x.PropertyName == "Created");
        }

        [DataTestMethod]
        [FilterFuncDataSource(nameof(GetEntityFilterFunctions), typeof(ModelDto))]
        public void WhenEntityFilterIsCast_ThenCastFilterCanBeCreatedAndMatches(EntityFilterFunc<ModelDto> filterFunc)
        {
            var modelFilter = new EntityFilter<Model>()
                .Add(x => x.Name, "Joe")
                .Add(x => x.Age, 20)
                .Add(x => x.Created, new DateTime(2000, 01, 01));

            var dtoFilter = modelFilter.Cast<ModelDto>();

            var testItems = new ModelDto[]
            {
                new() { Name = "Joe", Age = 20, Created = new DateTime(2000, 01, 01) },
                new() { Name = "Joe", Age = 20, Created = new DateTime(2020, 01, 01) },
                new() { Name = "Liz", Age = 20, Created = new DateTime(2020, 01, 01) }
            };

            var filteredItems = filterFunc(testItems, dtoFilter);
            filteredItems.Should().BeEquivalentTo(new[] { testItems[0] });
        }

        public class ModelDto
        {
            public Guid Id { get; set; }
            public string Name { get; init; }
            public short Age { get; init; }
            public DateTime? Created { get; init; }
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        public class Model
        {
            public Guid Id { get; set; }
            public string Name { get; }
            public int Age { get; }
            public DateTime Created { get; }
        }
        // ReSharper restore UnassignedGetOnlyAutoProperty
    }
}
