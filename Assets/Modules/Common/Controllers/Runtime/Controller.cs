using System;
using Modules.Common.Inputs.Runtime;
using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public abstract class Controller : MonoBehaviour
    {
        private bool paused;
        protected IInput Input;

        public void SetInput(IInput newInput) => Input = newInput;
        protected abstract void OnUpdate();
        private void Update()
        {
            if (paused) return;
            OnUpdate();
        }

        // Events callbacks
        public void OnGameStart() => paused = true;
        public void OnGamePause() => paused = true;
        public void OnGameResume() => paused = false;
        public void OnGameEnd() => paused = false;
    }
}