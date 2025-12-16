using UnityEngine;

public sealed partial class Game : MonoBehaviour
{
    private void Replay_OnEnter(GameState previousState)
    {
        App.Instance.StartGame();
    }
}
