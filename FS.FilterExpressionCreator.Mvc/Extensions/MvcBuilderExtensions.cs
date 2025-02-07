﻿using FS.FilterExpressionCreator.Mvc.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FS.FilterExpressionCreator.Mvc.Extensions
{
    /// <summary>
    /// Extensions to register entity filter extensions to MVC
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Registers filter expression creators specific model binders.
        /// </summary>
        /// <param name="mvcBuilder">The MVC builder.</param>
        /// <autogeneratedoc />
        public static IMvcBuilder AddFilterExpressionsSupport(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0, new EntityFilterModelBinderProvider());
                options.ModelBinderProviders.Insert(0, new EntityFilterSetModelBinderProvider());
            });

            return mvcBuilder;
        }
    }
}
