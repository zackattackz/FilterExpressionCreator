﻿using FS.FilterExpressionCreator.Abstractions.Models;
using FS.FilterExpressionCreator.Enums;
using FS.FilterExpressionCreator.Extensions;
using FS.FilterExpressionCreator.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace FS.FilterExpressionCreator.ValueFilterExpressionCreators
{
    /// <inheritdoc cref="IEnumFilterExpressionCreator"/>
    public class EnumFilterExpressionCreator : DefaultFilterExpressionCreator, IEnumFilterExpressionCreator
    {
        /// <inheritdoc />
        public override ICollection<FilterOperator> SupportedFilterOperators
            => new[]
            {
                FilterOperator.Default,
                FilterOperator.Contains,
                FilterOperator.EqualCaseSensitive,
                FilterOperator.EqualCaseInsensitive,
                FilterOperator.NotEqual,
                FilterOperator.LessThan,
                FilterOperator.LessThanOrEqual,
                FilterOperator.GreaterThan,
                FilterOperator.GreaterThanOrEqual,
                FilterOperator.IsNull,
                FilterOperator.NotNull,
            };

        /// <inheritdoc />
        public override bool CanCreateExpressionFor(Type type)
            => type.GetUnderlyingType().IsEnum;

        /// <inheritdoc />
        protected internal override Expression CreateExpressionForValue<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, FilterOperator filterOperator, string value, FilterConfiguration configuration)
        {
            if (long.TryParse(value, NumberStyles.Any, configuration.CultureInfo, out var numericValue))
                return CreateEnumExpressionByFilterOperator(propertySelector, filterOperator, numericValue);

            return CreateEnumFromStringExpression(propertySelector, filterOperator, value, configuration);
        }

        private Expression CreateEnumFromStringExpression<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, FilterOperator filterOperator, string value, FilterConfiguration configuration)
        {
            var enumValues = GetEnumValuesMatchByStringFilter<TProperty>(filterOperator, value, configuration).ToList();
            if (!enumValues.Any())
                return Expression.Constant(false);

            var result = enumValues
                .Select(x => CreateEnumExpressionByFilterOperator(propertySelector, filterOperator, x))
                .Aggregate(Expression.OrElse);

            return result;
        }

        private Expression CreateEnumExpressionByFilterOperator<TEntity, TProperty, TEnum>(Expression<Func<TEntity, TProperty>> propertySelector, FilterOperator filterOperator, TEnum value)
        {
            var underlyingEnumType = Enum.GetUnderlyingType(typeof(TProperty).GetUnderlyingType());
            var numericEnumValue = Convert.ChangeType(value, underlyingEnumType);

            switch (filterOperator)
            {
                case FilterOperator.Default:
                case FilterOperator.Contains:
                case FilterOperator.EqualCaseInsensitive:
                case FilterOperator.EqualCaseSensitive:
                    return CreateEqualExpression(propertySelector, value);
                case FilterOperator.NotEqual:
                    return CreateNotEqualExpression(propertySelector, value);
                case FilterOperator.LessThan:
                    return CreateLessThanExpression(propertySelector, underlyingEnumType, numericEnumValue);
                case FilterOperator.LessThanOrEqual:
                    return CreateLessThanOrEqualExpression(propertySelector, underlyingEnumType, numericEnumValue);
                case FilterOperator.GreaterThan:
                    return CreateGreaterThanExpression(propertySelector, underlyingEnumType, numericEnumValue);
                case FilterOperator.GreaterThanOrEqual:
                    return CreateGreaterThanOrEqualExpression(propertySelector, underlyingEnumType, numericEnumValue);
                default:
                    throw CreateFilterExpressionCreationException($"Filter operator '{filterOperator}' not allowed for property type '{typeof(TProperty)}'", propertySelector, filterOperator, value);
            }
        }

        private static IEnumerable<TProperty> GetEnumValuesMatchByStringFilter<TProperty>(FilterOperator filterOperator, string value, FilterConfiguration configuration)
        {
            var underlyingEnumType = typeof(TProperty).GetUnderlyingType();
            var enumValueToNameMap = Enum
                .GetValues(underlyingEnumType)
                .Cast<TProperty>()
                .ToDictionary(key => key, val => Enum.GetName(underlyingEnumType, val));

            var stringFilterExpressionCreator = new StringFilterExpressionCreator();
            Expression<Func<KeyValuePair<TProperty, string>, string>> dictionaryValueSelector = x => x.Value;

            var stringFilterOperator = filterOperator switch
            {
                FilterOperator.Contains => FilterOperator.Contains,
                FilterOperator.EqualCaseInsensitive => FilterOperator.EqualCaseInsensitive,
                FilterOperator.EqualCaseSensitive => FilterOperator.EqualCaseSensitive,
                _ => FilterOperator.EqualCaseInsensitive
            };

            var stringFilterExpression = stringFilterExpressionCreator.CreateExpressionForValue(dictionaryValueSelector, stringFilterOperator, value, configuration);

            var stringFilter = Expression
                .Lambda<Func<KeyValuePair<TProperty, string>, bool>>(stringFilterExpression, dictionaryValueSelector.Parameters)
                .Compile();

            var filteredValues = enumValueToNameMap
                .Where(stringFilter)
                .ToDictionary(x => x.Key, x => x.Value);

            return filteredValues.Keys;
        }
    }
}
