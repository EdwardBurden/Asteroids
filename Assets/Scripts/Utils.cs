using UnityEngine;

public static class Utils
{
    public static void DestoryAllChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}