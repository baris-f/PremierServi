using System.Collections.Generic;
using Modules.Technical.ScriptableCollections.Runtime;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime.Modes
{
    public class ModesCollection : ScriptableCollection<ModeDescriptor>
    {
        [SerializeField] private List<ModeDescriptor> selectedModes;
        [SerializeField] private List<ModeDescriptor> modes = new();

        public override IEnumerable<ModeDescriptor> Collection => modes;

#if UNITY_EDITOR
        [Button(header: "Cake Collection Functions", horizontal: true)]
        private void CreateNewMode()
        {
            var mode = New();
            mode.name = $"Mode {modes.Count}";
            RefreshCollection();
        }

        [Button(horizontal: true)]
        private void RemoveSelectedModes()
        {
            foreach (var selectedMode in selectedModes)
                Remove(selectedMode);
            selectedModes.Clear();
            RefreshCollection();
        }
        
        [Button]
        private void ApplyDisplayNames()
        {
            foreach (var mode in modes) mode.ApplyNameChange();
            RefreshCollection();
        }
        
        protected override void AddToCollection(ModeDescriptor obj) => modes.Add(obj);
        protected override void RemoveFromCollection(ModeDescriptor obj) => modes.Remove(obj);
#endif
    }
}