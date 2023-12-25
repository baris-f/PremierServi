using Modules.Technical.ScriptableCollections.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEditor;
using UnityEngine;

namespace Modules.Technical.ScriptableCollections.Editor
{
    public static class ScriptableCollectionExtension
    {
        public static void Add<T>(this ScriptableCollection<T> collection, T obj) where T : ScriptableObject
        {
            var existing = !string.IsNullOrWhiteSpace(AssetDatabase.GetAssetPath(obj));
            var newObj = existing ? AddExistingObject(obj) : obj;
            AssetDatabase.AddObjectToAsset(newObj, collection);
            collection.collection.Add(newObj);
            AssetDatabase.ImportAsset(collection.GetPath());
        }

        private static T AddExistingObject<T>(T obj) where T : ScriptableObject
        {
            var objPath = AssetDatabase.GetAssetPath(obj);
            var clone = obj.Clone();
            AssetDatabase.DeleteAsset(objPath);
            return clone;
        }

        private static string GetPath<T>(this ScriptableCollection<T> collection) where T : ScriptableObject
        {
            if (string.IsNullOrWhiteSpace(collection.path)
                || AssetDatabase.LoadAssetAtPath<ScriptableCollection<T>>(collection.path) == null)
                collection.path = AssetDatabase.GetAssetPath(collection);
            return collection.path;
        }
    }
}