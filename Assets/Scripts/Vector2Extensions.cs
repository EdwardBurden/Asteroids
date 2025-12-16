using Unity.VisualScripting;
using UnityEngine;

public static class Vector2Extensions 
{
    public static Vector3 ToVector3(this Vector2 vector) 
    {
        return new Vector3(vector.x, vector.y, 0f);
    }

    public static Vector3 Random2DVectorIn3D() 
    {
        return new Vector3(UnityEngine.Random.Range(1f, -1f), UnityEngine.Random.Range(1f, -1f), 0).normalized;
    }
}
