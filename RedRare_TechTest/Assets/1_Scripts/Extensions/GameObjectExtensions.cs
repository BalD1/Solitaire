using Unity.VisualScripting;
using UnityEngine;

public static class GameObjectExtensions
{
    #region Create

    public static GameObject Create(this GameObject gameObject)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject);
        return gO;
    }
    public static T Create<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject);
        return gO.GetComponent<T>();
    }

    public static GameObject Create(this GameObject gameObject, Vector2 position)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, position, Quaternion.identity);
        return gO;
    }
    public static GameObject Create(this GameObject gameObject, Vector2 position, Quaternion quaternion)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, position, quaternion);
        return gO;
    }

    public static GameObject Create(this GameObject gameObject, Vector3 position)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, position, Quaternion.identity);
        return gO;
    }
    public static GameObject Create(this GameObject gameObject, Vector3 position, Quaternion quaternion)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, position, quaternion);
        return gO;
    }

    public static GameObject Create(this GameObject gameObject, Transform parent)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, parent);
        return gO;
    }
    public static GameObject Create(this GameObject gameObject, RectTransform parent)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, parent);
        return gO;
    } 

    #endregion
}
