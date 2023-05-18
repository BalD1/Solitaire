using UnityEngine;

public interface IClickable
{
    public GameObject GetGameObject();

    public void OnMouseInputDown();
    public void OnMouseInputUp();
}