using System;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Runtime
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string Text { get; private set; }
        public string Header { get; private set; }

        public ButtonAttribute(string text = null, string header = null)
        {
            Text = text;
            Header = header;
        }
    }
}