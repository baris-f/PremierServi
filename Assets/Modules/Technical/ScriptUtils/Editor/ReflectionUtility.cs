using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Editor
{
    public static class ReflectionUtility
    {
        public static IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                yield break;
            }

            var types = GetSelfAndBaseTypes(target);
            for (var i = types.Count - 1; i >= 0; i--)
            {
                var methodInfos = types[i]
                    .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(predicate);
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