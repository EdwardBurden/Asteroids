using UnityEngine;

public sealed class LevelBoundedObject : MonoBehaviour
{
    private void Update()
    {
        if (ServiceLocator.GetService<LevelManager>().IsGameObjectInBounds(this.transform.gameObject))
            return;

        var newPos = ServiceLocator.GetService<LevelManager>().CalculateMirroredPosition(this.transform.gameObject);
        this.transform.position = newPos;
    }
}
