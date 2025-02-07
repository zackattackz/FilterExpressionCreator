﻿using FS.FilterExpressionCreator.Enums;
using FS.FilterExpressionCreator.Exceptions;
using FS.FilterExpressionCreator.Tests.Attributes;
using FS.FilterExpressionCreator.Tests.Extensions;
using FS.FilterExpressionCreator.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FS.FilterExpressionCreator.Tests.Tests.TypeFilter
{
    [TestClass, ExcludeFromCodeCoverage]
    public class FilterForGuidNullableByValueTests : TestBase<Guid?>
    {
        [DataTestMethod]
        [FilterTestDataSource(nameof(_testCases), nameof(TestModelFilterFunctions))]
        public void FilterForGuidNullableByValue_WorksAsExpected(FilterTestCase<Guid?, Guid?> testCase, TestModelFilterFunc<Guid?> filterFunc)
            => testCase.Run(_testItems, filterFunc);

        private static readonly TestModel<Guid?>[] _testItems = {
            new() { ValueA = null },
            new() { ValueA = Guid.Parse("df72ce74-686c-4c0f-a11f-5c8e50a213ab") },
            new() { ValueA = Guid.Parse("6cda682c-e7ff-43e8-b4d9-f8b27a7d62f2") },
            new() { ValueA = Guid.Parse("6cda682c-c0cc-4d4d-bea3-7058777e98d1") },
        };

        // ReSharper disable RedundantExplicitArrayCreation
        private static readonly FilterTestCase<Guid?, Guid?>[] _testCases = {
            FilterTestCase.Create(1100, FilterOperator.Default, new Guid?[] { Guid.Parse("6cda682c-e7ff-43e8-b4d9-f8b27a7d62f2") }, (Guid? x) => x == Guid.Parse("6cda682c-e7ff-43e8-b4d9-f8b27a7d62f2")),

            FilterTestCase.Create(1200, FilterOperator.Contains, new Guid?[] { Guid.Parse("df72ce74-686c-4c0f-a11f-5c8e50a213ab") }, (Guid? x) => x == Guid.Parse("df72ce74-686c-4c0f-a11f-5c8e50a213ab")),

            FilterTestCase.Create(1300, FilterOperator.EqualCaseInsensitive, new Guid?[] { Guid.Parse("6cda682c-e7ff-43e8-b4d9-f8b27a7d62f2") }, (Guid? x) => x == Guid.Parse("6cda682c-e7ff-43e8-b4d9-f8b27a7d62f2")),

            FilterTestCase.Create(1400, FilterOperator.EqualCaseSensitive, new Guid?[] { Guid.Parse("6cda682c-e7ff-43e8-b4d9-f8b27a7d62f2") }, (Guid? x) => x == Guid.Parse("6cda682c-e7ff-43e8-b4d9-f8b27a7d62f2")),

            FilterTestCase.Create(1500, FilterOperator.NotEqual, new Guid?[] { Guid.Parse("6cda682c-e7ff-43e8-b4d9-f8b27a7d62f2") }, (Guid? x) => x != Guid.Parse("6cda682c-e7ff-43e8-b4d9-f8b27a7d62f2")),

            FilterTestCase.Create(1600, FilterOperator.LessThan, new Guid?[] { Guid.Empty }, new FilterExpressionCreationException("Filter operator 'LessThan' not allowed for property type 'System.Nullable`1[System.Guid]'")),

            FilterTestCase.Create(1700, FilterOperator.LessThanOrEqual, new Guid?[] { Guid.Empty }, new FilterExpressionCreationException("Filter operator 'LessThanOrEqual' not allowed for property type 'System.Nullable`1[System.Guid]'")),

            FilterTestCase.Create(1800, FilterOperator.GreaterThan, new Guid?[] { Guid.Empty }, new FilterExpressionCreationException("Filter operator 'GreaterThan' not allowed for property type 'System.Nullable`1[System.Guid]'")),

            FilterTestCase.Create(1900, FilterOperator.GreaterThanOrEqual, new Guid?[] { Guid.Empty }, new FilterExpressionCreationException("Filter operator 'GreaterThanOrEqual' not allowed for property type 'System.Nullable`1[System.Guid]'")),

            FilterTestCase.Create(2000, FilterOperator.IsNull, new Guid?[] { default }, (Guid? x) => x == null),

            FilterTestCase.Create(2100, FilterOperator.NotNull, new Guid?[] { default }, (Guid? x) => x != null),
        };
        // ReSharper restore RedundantExplicitArrayCreation
    }
}
