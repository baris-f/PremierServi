using System.Linq;

namespace Modules.Technical.SceneLoader.Editor
{
    public class SceneAssetProcessor 
        // : AssetPostprocessor // -> disable to avoid slowing off
    {
        //todo implement ? but need a good way to access the scene event asset
        // hardcoded path for access to the scenevent asset.
        // todo put it in Settings or something -> make a thing for easy settings 
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            var allAssets = importedAssets.Concat(deletedAssets).Concat(movedAssets).Concat(movedFromAssetPaths);
            
        }
    }
}