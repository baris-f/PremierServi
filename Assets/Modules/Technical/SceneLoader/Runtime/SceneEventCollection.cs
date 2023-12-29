using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Modules.Technical.ScriptableCollections.Editor;
using Modules.Technical.ScriptableCollections.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
#if UNITY_EDITOR
using Modules.Technical.ScriptableEvents.Runtime.Inspectors;
using UnityEditor;
using UnityEngine.SceneManagement;
#endif

namespace Modules.Technical.SceneLoader.Runtime
{
    public class SceneEventCollection : ScriptableCollection<LoadSceneEvent>
    {
        [Header("Debug")]
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private List<LoadSceneEvent> notInBuildSettings = new();

#if UNITY_EDITOR
        [Button] private void RefreshCollection()
        {
            var scenesGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" }).ToList();

            foreach (var sceneGuid in scenesGuids)
            {
                if (collection.Exists(@event => @event.sceneGuid == sceneGuid)) continue;
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                var sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                var sceneEvent = this.New();
                sceneEvent.Initialize(sceneGuid, scenePath, sceneName);
                Debug.Log($"Adding Event for new scene {sceneName}");
            }

            notInBuildSettings.Clear();
            collection.RemoveAll(@event =>
            {
                var buildIndex = SceneUtility.GetBuildIndexByScenePath(@event.scenePath);
                if (buildIndex == -1)
                {
                    notInBuildSettings.Add(@event);
                    Debug.LogWarning($"Scene {@event.scenePath} is not present in build settings");
                }

                var guidValid = !string.IsNullOrWhiteSpace(AssetDatabase.GUIDToAssetPath(@event.sceneGuid));
                return !@event.Valid || !guidValid;
            });
            this.Cleanup();
        }

        [Button] private void RefreshSceneLoader()
        {
            var listener = sceneLoader.GetComponent<MultiEventsListener>();
            listener.Events.Clear();
            listener.Events.AddRange(collection);
        }
#endif
    }
}