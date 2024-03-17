using System.Collections.Generic;
using Modules.Technical.GameConfig.Runtime.RoundsProvider;
using UnityEngine;

namespace Modules.Scenes._0___MainMenu.Runtime.Choices
{
    public class RoundsProviderChoice : OptionsProvider, IGetResult<BaseRoundsProvider>
    {
        [Header("Choices")]
        [SerializeField] private List<BaseRoundsProvider> providers = new();

        private readonly List<string> options = new();
        public override List<string> Options => options;

        public BaseRoundsProvider GetResult(int id)
            => providers[id];

        private void OnValidate()
        {
            options.Clear();
            foreach (var provider in providers)
                options.Add(provider.Name);
        }
    }
}