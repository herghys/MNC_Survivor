using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Utility.Logger;

using UnityEngine;

namespace HerghysStudio.Survivor.Utility.Coroutines
{
    public class CoroutineHelper : Singletons.PersistentSingleton<CoroutineHelper>
    {
        [RuntimeInitializeOnLoadMethod()]
        public static void CreateInstance()
        {
            //Setup new instance and Hide from hierarchy
            var go = new GameObject("CoroutineHelper", typeof(CoroutineHelper));
            go.transform.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    public static class CoroutineExtensions
    {
        private static Dictionary<IEnumerator, UnityEngine.Coroutine> activeCoroutines = new Dictionary<IEnumerator, UnityEngine.Coroutine>();
        public static UnityEngine.Coroutine Run(this IEnumerator coroutine, MonoBehaviour monoBehaviour = null)
        {
            // Use the provided MonoBehaviour or find an active one in the scene
            if (monoBehaviour == null)
            {
                //Use the coroutine helper
                monoBehaviour = CoroutineHelper.Instance;
                if (monoBehaviour == null)
                {
                    GameLogger.LogError("No MonoBehaviour found to start the coroutine.");
                    return null;
                }
            }

            // Start the coroutine and track it
            UnityEngine.Coroutine runningCoroutine = monoBehaviour.StartCoroutine(coroutine);
            if (!activeCoroutines.ContainsKey(coroutine))
            {
                activeCoroutines[coroutine] = runningCoroutine;
            }

            return runningCoroutine;
        }

        public static void Stop(this IEnumerator coroutine, MonoBehaviour monoBehaviour = null)
        {
            if (monoBehaviour == null)
            {
                monoBehaviour = Object.FindFirstObjectByType<MonoBehaviour>();
                if (monoBehaviour == null)
                {
                    Debug.LogError("No MonoBehaviour found to stop the coroutine.");
                    return;
                }
            }

            if (activeCoroutines.TryGetValue(coroutine, out Coroutine runningCoroutine))
            {
                monoBehaviour.StopCoroutine(runningCoroutine);
                activeCoroutines.Remove(coroutine);
            }
            else
            {
                Debug.LogWarning("Coroutine not found or already stopped.");
            }
        }
    }
}
