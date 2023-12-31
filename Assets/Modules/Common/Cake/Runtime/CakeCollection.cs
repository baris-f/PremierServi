using System.Collections.Generic;
using Modules.Technical.ScriptableCollections.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;

namespace Modules.Common.Cake.Runtime
{
    public class CakeCollection : ScriptableCollection<Cake>
    {
        [SerializeField] private List<Cake> selectedCakes;
        [SerializeField] private List<Cake> cakes = new();
        public override IEnumerable<Cake> Collection => cakes;

        public List<Cake> CakesOfType(Cake.CakeType type) => cakes.FindAll(cake => cake.CompareType(type));

        public Cake GetRandomCake(Cake.CakeType type = Cake.CakeType.All)
        {
            return CakesOfType(type).ToArray().GetRandom();
        }

#if UNITY_EDITOR
        [Button(header: "Cake Collection Functions", horizontal: true)]
        private void CreateNewCake()
        {
            var cake = New();
            cake.Initialize($"Cake {cakes.Count}");
            RefreshCollection();
        }

        [Button(horizontal: true)]
        private void RemoveSelectedCakes()
        {
            foreach (var selectedCake in selectedCakes)
                Remove(selectedCake);
            selectedCakes.Clear();
            RefreshCollection();
        }

        [Button]
        private void ApplyDisplayNames()
        {
            foreach (var cake in cakes) cake.ApplyNameChange();
            RefreshCollection();
        }

        protected override void AddToCollection(Cake obj) => cakes.Add(obj);
        protected override void RemoveFromCollection(Cake obj) => cakes.Remove(obj);
#endif
    }
}