using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Threading.Tasks;

namespace GamevrestUtils
{
    public static class GamevrestTools
    {
        public static void FirebaseTaskReport(Task task)
        {
            if (task.IsCanceled)
                Debug.LogWarning($"task canceled : \n{task.Exception}");
            else if (task.IsFaulted)
                Debug.LogWarning($"task faulted : \n{task.Exception}");
            else if (task.IsCompleted)
                Debug.Log($"task successful");
        }
        
        public static string GetTimestamp(DateTime date) => date.ToString("yyyyMMddHHmmss");

        public static float RoundFloat(float nbToRound, int decimalsToKeep = 1)
        {
            var div = Mathf.Pow(10, decimalsToKeep);
            return (int) (nbToRound * div) / div;
        }

        public static void CleanChildren(Transform parent)
        {
#if UNITY_EDITOR
            var toDelete = new List<GameObject>();
            foreach (Transform transform in parent)
                toDelete.Add(transform.gameObject);
            foreach (var child in toDelete)
                Object.DestroyImmediate(child);
#else
            foreach (Transform child in parent)
                Object.Destroy(child.gameObject);
#endif
        }

        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}