using UnityEngine;

public static class Utils
{
    public static void DestoryAllChildren(Transform transform)
    {
        if (transform == null || transform.childCount == 0)
            return;

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}