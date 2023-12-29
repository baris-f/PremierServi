using System;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string Text { get; }
        public string Header { get; }
        public bool Horizontal { get; }

        public ButtonAttribute(string text = null, string header = null, bool horizontal = false)
        {
            Text = text;
            Header = header;
            Horizontal = horizontal;
        }
    }
}