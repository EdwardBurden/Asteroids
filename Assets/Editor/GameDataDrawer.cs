using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameData))]
public class GameDataDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Fill All"))
        {
        }
    }

    private void FindAllGameData() 
    {

 //todo
    }
}
