﻿using FS.FilterExpressionCreator.Abstractions.Extensions;
using FS.FilterExpressionCreator.Abstractions.Models;
using FS.FilterExpressionCreator.Filters;
using FS.FilterExpressionCreator.Interfaces;
using FS.FilterExpressionCreator.ValueFilterExpressionCreators;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FS.FilterExpressionCreator.PropertyFilterExpressionCreators
{
    /// <summary>
    /// Converter to create lambda filter expressions for a given property and a <see cref="ValueFilter"/>.
    /// </summary>
    public static class PropertyFilterExpressionCreator
    {
        private static readonly IValueFilterExpressionCreator _defaultValueFilterExpressionCreator = new DefaultFilterExpressionCreator();
        private static readonly MethodInfo _createFilterMethod = typeof(PropertyFilterExpressionCreator).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.Name == nameof(CreateFilter) && x.IsGenericMethod && x.GetGenericArguments().Length == 2);

        private static readonly IValueFilterExpressionCreator[] _valueFilterExpressionCreators = {
            new StringFilterExpressionCreator(),
            new GuidFilterExpressionCreator(),
            new DateTimeFilterExpressionCreator(),
            new BooleanFilterExpressionCreator(),
            new NumericFilterExpressionCreator(),
            new EnumFilterExpressionCreator(),
        };

        /// <summary>
        /// Determines whether a property of type <paramref name="propertyType"/> can be filtered.
        /// </summary>
        /// <param name="propertyType">The type to filter.</param>
        public static bool CanCreateFilterFor(Type propertyType)
            => _valueFilterExpressionCreators.Any(x => x.CanCreateExpressionFor(propertyType));

        /// <summary>
        /// Creates a lambda expression for the given property and <see cref="ValueFilter"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <param name="valueFilters">The filters to use.</param>
        /// <param name="configuration">The filter configuration.</param>
        public static Expression<Func<TEntity, bool>> CreateFilter<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, ValueFilter[] valueFilters, FilterConfiguration configuration)
        {
            var valueFilterExpressionCreator = _valueFilterExpressionCreators.FirstOrDefault(x => x.CanCreateExpressionFor<TProperty>()) ?? _defaultValueFilterExpressionCreator;
            var propertyExpression = valueFilterExpressionCreator.CreateExpression(propertySelector, valueFilters, configuration);
            if (propertyExpression == null)
                return null;

            var result = propertySelector.CreateLambda<TEntity, TProperty, bool>(propertyExpression);
            return result;
        }

        /// <summary>
        /// Creates a lambda expression for the given property and <see cref="ValueFilter"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="propertySelector">The property selector.</param>
        /// <param name="valueFilters">The filters to use.</param>
        /// <param name="configuration">The filter configuration.</param>
        public static Expression<Func<TEntity, bool>> CreateFilter<TEntity>(Type propertyType, LambdaExpression propertySelector, ValueFilter[] valueFilters, FilterConfiguration configuration)
        {
            try
            {
                var genericMethod = _createFilterMethod!.MakeGenericMethod(typeof(TEntity), propertyType);
                var propertyExpression = (Expression<Func<TEntity, bool>>)genericMethod.Invoke(null, new object[] { propertySelector, valueFilters, configuration });
                return propertyExpression;
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                throw ex.InnerException;
            }
        }
    }
}
