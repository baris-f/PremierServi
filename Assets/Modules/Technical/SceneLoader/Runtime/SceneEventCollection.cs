using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Modules.Technical.ScriptableCollections.Editor;
using Modules.Technical.ScriptableCollections.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using System.IO;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Modules.Technical.SceneLoader.Runtime
{
    public class SceneEventCollection : ScriptableCollection<LoadSceneEvent>
    {
        [Header("Debug")]
        [SerializeField] private List<LoadSceneEvent> notInBuildSettings = new();

#if UNITY_EDITOR
        [Button] public void RefreshCollection()
        {
            var scenesGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" }).ToList();
            foreach (var sceneGuid in scenesGuids)
            {
                if (collection.Exists(@event => @event.sceneGuid == sceneGuid)) continue;
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                var sceneEvent = this.New();
                sceneEvent.Initialize(sceneGuid, scenePath, sceneName);
                Debug.Log($"Adding Event for new scene {sceneName}");
            }

            notInBuildSettings.Clear();
            collection.RemoveAll(@event =>
            {
                var sceneExists = AssetDatabase.LoadAssetAtPath<Object>(@event.scenePath) != null;
                if (!sceneExists || !@event.Valid) return true;
                var buildIndex = SceneUtility.GetBuildIndexByScenePath(@event.scenePath);
                if (buildIndex > 0) return false;
                notInBuildSettings.Add(@event);
                Debug.LogWarning($"Scene {@event.scenePath} is not present in build settings");
                return false;
            });
            this.Cleanup();
        }
#endif
    }
}