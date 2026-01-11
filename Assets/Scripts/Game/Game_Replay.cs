using UnityEngine;

public sealed partial class Game 
{
    private void Replay_OnEnter(GameState previousState)
    {
        App.Instance.StartGame(); //temp, should use the state machine to restart game, will do for now.
    }
}
