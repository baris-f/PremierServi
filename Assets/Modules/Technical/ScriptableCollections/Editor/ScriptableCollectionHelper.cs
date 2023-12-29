using System.Linq;
using Modules.Technical.ScriptableCollections.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEditor;
using UnityEngine;

namespace Modules.Technical.ScriptableCollections.Editor
{
    public static class ScriptableCollectionExtension
    {
        public static T New<T>(this ScriptableCollection<T> collection) where T : ScriptableObject
        {
            var newObj = ScriptableObject.CreateInstance(typeof(T)) as T;
            AddToCollection(collection, newObj);
            return newObj;
        }

        public static void Add<T>(this ScriptableCollection<T> collection, T obj) where T : ScriptableObject
        {
            var objPath = AssetDatabase.GetAssetPath(obj);
            var existing = !string.IsNullOrWhiteSpace(objPath);
            if (existing)
            {
                AssetDatabase.DeleteAsset(objPath);
                AddToCollection(collection, obj.Clone());
            }
            else
                AddToCollection(collection, obj);
        }

        public static void Release<T>(this ScriptableCollection<T> collection, T obj) where T : ScriptableObject
        {
            var exists = collection.collection.Find(o => o == obj);
            if (!exists) return;
            var clone = obj.Clone();
            var path = collection.GetPath();
            path = $"{path.Remove(path.LastIndexOf('/'))}/{obj.name}.asset";
            AssetDatabase.CreateAsset(clone, path);
            AssetDatabase.ImportAsset(path);
            RemoveFromCollection(collection, obj);
        }

        public static void Remove<T>(this ScriptableCollection<T> collection, T obj) where T : ScriptableObject
        {
            var exists = collection.collection.Find(o => o == obj);
            if (!exists) return;
            RemoveFromCollection(collection, obj);
        }

        public static void Cleanup<T>(this ScriptableCollection<T> collection) where T : ScriptableObject
        {
            var children = AssetDatabase.LoadAllAssetRepresentationsAtPath(collection.GetPath());
            foreach (var child in children)
            {
                if (!collection.collection.Contains(child))
                    AssetDatabase.RemoveObjectFromAsset(child);
            }

            AssetDatabase.ImportAsset(collection.GetPath());
        }

        private static void AddToCollection<T>(ScriptableCollection<T> collection, T obj) where T : ScriptableObject
        {
            collection.collection.Add(obj);
            AssetDatabase.AddObjectToAsset(obj, collection);
            AssetDatabase.ImportAsset(collection.GetPath());
        }

        private static void RemoveFromCollection<T>(ScriptableCollection<T> collection, T obj)
            where T : ScriptableObject
        {
            collection.collection.Remove(obj);
            AssetDatabase.RemoveObjectFromAsset(obj);
            AssetDatabase.ImportAsset(collection.GetPath());
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