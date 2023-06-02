using System;
using UnityEngine;

namespace Modules.ScriptUtils.Runtime
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string Text { get; private set; }

        public ButtonAttribute(string text = null)
        {
            Text = text;
            if (string.IsNullOrWhiteSpace(text))
            {
            }
        }
    }
}