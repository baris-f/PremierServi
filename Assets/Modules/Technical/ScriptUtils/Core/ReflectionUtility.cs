using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Core
{
    public static class ReflectionUtility
    {
        public static IEnumerable<FieldInfo> GetAllFields(object target, BindingFlags flags,Func<FieldInfo, bool> predicate)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                yield break;
            }
            
            var types = GetSelfAndBaseTypes(target);
            foreach (var type in types)
            {
                var propertyInfos = type
                    .GetFields(flags).Where(predicate);
                foreach (var propertyInfo in propertyInfos)
                    yield return propertyInfo;
            }
        }

        public static IEnumerable<MethodInfo> GetAllMethods(object target, BindingFlags flags,Func<MethodInfo, bool> predicate)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                yield break;
            }

            var types = GetSelfAndBaseTypes(target);
            foreach (var type in types)
            {
                var methodInfos = type
                    .GetMethods(flags).Where(predicate);
                foreach (var methodInfo in methodInfos)
                    yield return methodInfo;
            }
        }

        private static List<Type> GetSelfAndBaseTypes(object target)
        {
            var types = new List<Type> { target.GetType() };
            while (types.Last().BaseType != null)
                types.Add(types.Last().BaseType);
            return types;
        }
    }
}