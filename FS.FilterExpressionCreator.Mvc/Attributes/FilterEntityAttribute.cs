﻿using FS.FilterExpressionCreator.Filters;
using System;

namespace FS.FilterExpressionCreator.Mvc.Attributes
{
    /// <summary>
    /// Entity filter expression related settings.
    /// Implements <see cref="Attribute" />
    /// </summary>
    /// <seealso cref="Attribute" />
    /// <autogeneratedoc />
    [AttributeUsage(AttributeTargets.Class)]
    public class FilterEntityAttribute : Attribute
    {
        /// <summary>
        /// Specify the prefix to use when using <see cref="EntityFilter{TEntity}"/> from MVC controllers.
        /// Default is the name of the filtered class (e.g. for <c>EntityFilter&lt;Person&gt;</c> the prefix is <c>Person)</c>.
        /// </summary>
        public string Prefix { get; set; }
    }
}
