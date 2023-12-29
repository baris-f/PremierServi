using System;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
    public class DebugAttribute : PropertyAttribute
    {
        public bool CanToggle { get; }

        public DebugAttribute(bool canToggle = true)
        {
            CanToggle = canToggle;
        }
    }
}