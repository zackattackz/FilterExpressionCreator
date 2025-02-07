﻿using FS.FilterExpressionCreator.Exceptions;
using FS.FilterExpressionCreator.Tests.Attributes;
using FS.FilterExpressionCreator.Tests.Extensions;
using FS.FilterExpressionCreator.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace FS.FilterExpressionCreator.Tests.Tests.TypeFilter
{
    [TestClass, ExcludeFromCodeCoverage]
    public class FilterForBoolNullableBySyntaxTests : TestBase<bool?>
    {
        [DataTestMethod]
        [FilterTestDataSource(nameof(_testCases), nameof(TestModelFilterFunctions))]
        public void FilterForBoolNullableBySyntax_WorksAsExpected(FilterTestCase<bool?, bool?> testCase, TestModelFilterFunc<bool?> filterFunc)
            => testCase.Run(_testItems, filterFunc);

        private static readonly TestModel<bool?>[] _testItems = {
            new() { ValueA = true },
            new() { ValueA = false },
            new() { ValueA = null },
        };

        private static readonly FilterTestCase<bool?, bool?>[] _testCases = {
            FilterTestCase.Create<bool?>(1000, "null", _ => ALL, IgnoreParseExceptions),
            FilterTestCase.Create<bool?>(1001, "=null", _ => ALL, IgnoreParseExceptions),
            FilterTestCase.Create<bool?>(1002, "TRUE", x => x == true),
            FilterTestCase.Create<bool?>(1003, "FALSE", x => x == false),
            FilterTestCase.Create<bool?>(1004, "yes", x => x == true),
            FilterTestCase.Create<bool?>(1005, "no", x => x == false),
            FilterTestCase.Create<bool?>(1006, "YES", x => x == true),
            FilterTestCase.Create<bool?>(1007, "NO", x => x == false),
            FilterTestCase.Create<bool?>(1008, "1", x => x == true),
            FilterTestCase.Create<bool?>(1009, "0", x => x == false),

            FilterTestCase.Create<bool?>(1100, "null", new FilterExpressionCreationException("Unable to parse given filter value")),
            FilterTestCase.Create<bool?>(1101, "", new FilterExpressionCreationException("Unable to parse given filter value")),
            FilterTestCase.Create<bool?>(1102, "true", x => x == true),
            FilterTestCase.Create<bool?>(1103, "false", x => x == false),
            FilterTestCase.Create<bool?>(1104, "true, false", x => x == true || x == false),

            FilterTestCase.Create<bool?>(1200, "~", new FilterExpressionCreationException("Filter operator 'Contains' not allowed for property type 'System.Nullable`1[System.Boolean]'")),

            FilterTestCase.Create<bool?>(1300, "=null", new FilterExpressionCreationException("Unable to parse given filter value")),
            FilterTestCase.Create<bool?>(1301, "=", new FilterExpressionCreationException("Unable to parse given filter value")),
            FilterTestCase.Create<bool?>(1302, "=true", x => x == true),
            FilterTestCase.Create<bool?>(1303, "=false", x => x == false),
            FilterTestCase.Create<bool?>(1304, "=true, false", x => x == true || x == false),

            FilterTestCase.Create<bool?>(1400, "==null", new FilterExpressionCreationException("Unable to parse given filter value")),
            FilterTestCase.Create<bool?>(1401, "==", new FilterExpressionCreationException("Unable to parse given filter value")),
            FilterTestCase.Create<bool?>(1402, "==true", x => x == true),
            FilterTestCase.Create<bool?>(1403, "==false", x => x == false),
            FilterTestCase.Create<bool?>(1404, "==true, false", x => x == true || x == false),

            FilterTestCase.Create<bool?>(1500, "!null", new FilterExpressionCreationException("Unable to parse given filter value")),
            FilterTestCase.Create<bool?>(1501, "!", new FilterExpressionCreationException("Unable to parse given filter value")),
            FilterTestCase.Create<bool?>(1502, "!true", x => x != true),
            FilterTestCase.Create<bool?>(1503, "!false", x => x != false),
            FilterTestCase.Create<bool?>(1504, "!true, !false", _ => ALL),

            FilterTestCase.Create<bool?>(1600, "<", new FilterExpressionCreationException("Filter operator 'LessThan' not allowed for property type 'System.Nullable`1[System.Boolean]'")),

            FilterTestCase.Create<bool?>(1700, "<=", new FilterExpressionCreationException("Filter operator 'LessThanOrEqual' not allowed for property type 'System.Nullable`1[System.Boolean]'")),

            FilterTestCase.Create<bool?>(1800, ">", new FilterExpressionCreationException("Filter operator 'GreaterThan' not allowed for property type 'System.Nullable`1[System.Boolean]'")),

            FilterTestCase.Create<bool?>(1900, ">=", new FilterExpressionCreationException("Filter operator 'GreaterThanOrEqual' not allowed for property type 'System.Nullable`1[System.Boolean]'")),

            FilterTestCase.Create<bool?>(2000, "ISNULL", x => x == null),

            FilterTestCase.Create<bool?>(2100, "NOTNULL", x => x != null),
        };
    }
}
