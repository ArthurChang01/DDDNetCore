using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Fundamentals.Miscs
{
    public class ServiceFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly string _appNameSpace;
        private readonly string _boundedContext = string.Empty;

        public ServiceFeatureProvider(string boundedContext)
        {
            this._boundedContext = boundedContext;
            this._appNameSpace = $"PI.Domain.{this._boundedContext}";
        }

        /// <inheritdoc />
        public void PopulateFeature(
            IEnumerable<ApplicationPart> parts,
            ControllerFeature feature)
        {
            feature.Controllers.Clear();
            foreach (var part in parts.OfType<IApplicationPartTypeProvider>())
            {
                foreach (var type in part.Types)
                {
                    if (IsController(type) && !feature.Controllers.Contains(type))
                    {
                        feature.Controllers.Add(type);
                    }
                }
            }
        }

        /// <summary>
        /// Determines if a given <paramref name="typeInfo"/> is a controller.
        /// </summary>
        /// <param name="typeInfo">The <see cref="TypeInfo"/> candidate.</param>
        /// <returns><code>true</code> if the type is a controller; otherwise <code>false</code>.</returns>
        protected virtual bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            // We only consider public top-level classes as controllers. IsPublic returns false for nested
            // classes, regardless of visibility modifiers
            if (!typeInfo.IsPublic)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (!(new string[] { "Gateway" }).Any(o => typeInfo.Name.EndsWith(o)))
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }

            if (!typeInfo.Namespace.StartsWith(_appNameSpace, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}