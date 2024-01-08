using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Modules.Technical.ScriptableCollections.Runtime;
using System.IO;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
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
        [SerializeField] private List<LoadSceneEvent> scenes = new();

        public override IEnumerable<LoadSceneEvent> Collection => scenes;
        
        protected override void AddToCollection(LoadSceneEvent obj) => scenes.Add(obj);
        protected override void RemoveFromCollection(LoadSceneEvent obj) => scenes.Remove(obj);

#if UNITY_EDITOR
        [Button(header: "Scene Collection Functions")]
        public void ScanScenes()
        {
            var scenesGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" }).ToList();
            foreach (var sceneGuid in scenesGuids)
            {
                if (scenes.Exists(@event => @event.sceneGuid == sceneGuid)) continue;
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                var sceneEvent = New();
                sceneEvent.Initialize(sceneGuid, scenePath, sceneName);
                Debug.Log($"Adding Event for new scene {sceneName}");
            }

            notInBuildSettings.Clear();
            scenes.RemoveAll(@event =>
            {
                var sceneExists = AssetDatabase.LoadAssetAtPath<Object>(@event.scenePath) != null;
                if (!sceneExists || !@event.Valid) return true;
                var buildIndex = SceneUtility.GetBuildIndexByScenePath(@event.scenePath);
                if (buildIndex > 0) return false;
                notInBuildSettings.Add(@event);
                Debug.LogWarning($"Scene {@event.scenePath} is not present in build settings");
                return false;
            });
            Cleanup();
        }
#endif
    }
}