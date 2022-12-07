using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class ComponentManager
{
    private static readonly Dictionary<GameObject, Component> Components = new();

    static ComponentManager()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public static T GetCachedComponent<T>(this GameObject gameObject) where T : Component
    {
        if (Components.TryGetValue(gameObject, out Component component))
            return (T)component;

        component = gameObject.GetComponent<T>();
        Components.Add(gameObject, component);
        return (T)component;
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        Components.Clear();
    }
}