using UnityEngine;

public class LevelBoundedObject : MonoBehaviour
{
    private void Update()
    {
        if (Game.Instance.LevelManager.IsGameObjectInBounds(this.transform.gameObject))
            return;


        var newPos = Game.Instance.LevelManager.CalculateMirroredPosition(this.transform.gameObject);
        this.transform.position = newPos;
    }
}
