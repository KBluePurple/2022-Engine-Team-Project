using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AchromaticDev.Util
{
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static bool IsInitialized => _instance != null;
        protected static T _instance;
        private static readonly object Lock = new();
        private static bool _isDontDestroyOnLoad;

        public static T Instance
        {
            get
            {
                lock (Lock)
                {
                    if (!ReferenceEquals(_instance, null)) return _instance;

                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError($"[Singleton] {typeof(T)} More than one instance of singleton found!");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        var singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T);

                        _isDontDestroyOnLoad =
                            typeof(T).GetCustomAttributes(typeof(DontDestroyOnLoadAttribute), true).Length > 0;

                        if (_isDontDestroyOnLoad)
                            DontDestroyOnLoad(singleton);
                        else
                            SceneManager.sceneUnloaded += OnSceneUnloaded;

                        Debug.Log(
                            $"[Singleton] An instance of {typeof(T)} is needed in the scene, so '{singleton}' was created.");
                    }
                    else
                    {
                        Debug.Log($"[Singleton] Using instance already created: {_instance.gameObject.name}");
                    }

                    return _instance;
                }
            }
        }

        public virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                _isDontDestroyOnLoad =
                    typeof(T).GetCustomAttributes(typeof(DontDestroyOnLoadAttribute), true).Length > 0;

                if (_isDontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
                else
                    SceneManager.sceneUnloaded += OnSceneUnloaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (!_isDontDestroyOnLoad) _instance = null;
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            if (!_isDontDestroyOnLoad) _instance = null;
        }
    }
}