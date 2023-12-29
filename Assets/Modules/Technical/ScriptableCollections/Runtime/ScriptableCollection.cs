using System.Collections.Generic;
using System.Linq;
using Modules.Technical.ScriptUtils.Runtime;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Modules.Technical.ScriptableCollections.Runtime
{
    public abstract class ScriptableCollection<T> : ScriptableObject where T : ScriptableObject
    {
        public abstract IEnumerable<T> Collection { get; }
        private string path;
        private string Path
        {
            get
            {
                if (string.IsNullOrWhiteSpace(path)
                    || AssetDatabase.LoadAssetAtPath<ScriptableCollection<T>>(path) == null)
                    path = AssetDatabase.GetAssetPath(this);
                return path;
            }
        }

#if UNITY_EDITOR
        protected T New()
        {
            var newObj = CreateInstance(typeof(T)) as T;
            if (newObj == null) return null;
            AssetDatabase.AddObjectToAsset(newObj, this);
            AddToCollection(newObj);
            return newObj;
        }

        protected void Add(T obj)
        {
            var objPath = AssetDatabase.GetAssetPath(obj);
            var existing = !string.IsNullOrWhiteSpace(objPath);
            var newObj = obj;
            if (existing)
            {
                newObj = obj.Clone();
                AssetDatabase.DeleteAsset(objPath);
            }

            AssetDatabase.AddObjectToAsset(newObj, this);
            AddToCollection(newObj);
        }

        protected void Release(T obj)
        {
            var exists = Collection.FirstOrDefault(o => o == obj) != null;
            if (!exists) return;
            var clone = obj.Clone();
            var releasedPath = Path;
            releasedPath = $"{releasedPath.Remove(releasedPath.LastIndexOf('/'))}/{obj.name}.asset";
            AssetDatabase.CreateAsset(clone, releasedPath);
            AssetDatabase.ImportAsset(releasedPath);
            AssetDatabase.RemoveObjectFromAsset(obj);
            RemoveFromCollection(obj);
        }

        protected void Remove(T obj)
        {
            var exists = Collection.First(o => o == obj) != null;
            if (!exists) return;
            AssetDatabase.RemoveObjectFromAsset(obj);
            RemoveFromCollection(obj);
        }

        [Button(header:"ScriptableCollection Functions", horizontal:true)]
        protected void Cleanup()
        {
            var children = AssetDatabase.LoadAllAssetRepresentationsAtPath(Path);
            foreach (var child in children)
            {
                if (!Collection.Contains(child))
                    AssetDatabase.RemoveObjectFromAsset(child);
            }
            RefreshCollection();
        }

        [Button(horizontal: true)]
        protected void RefreshCollection() => AssetDatabase.ImportAsset(Path);

        protected abstract void AddToCollection(T obj);
        protected abstract void RemoveFromCollection(T obj);
#endif
    }
}