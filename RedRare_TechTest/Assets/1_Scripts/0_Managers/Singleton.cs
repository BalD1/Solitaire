using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance) return instance;

            T[] objs = FindObjectOfType(typeof(T)) as T[];

            if (objs.Length > 0) instance = objs[0];

            if (objs.Length > 1) Debug.LogErrorFormat($"There is more than one {typeof(T)} object");

            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}
