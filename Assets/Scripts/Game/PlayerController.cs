using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour, IGameSystem
{
    [SerializeField] private Transform m_playerRoot;
    [SerializeField] private GameObject m_debugLookVectorTarget;
    private Player m_player;

    public void Startup()
    {
        var playerPrefab = App.Instance.GameData.PlayerData.Prefab;
        m_player = GameObject.Instantiate(playerPrefab, m_playerRoot);
    }




    public void ShutDown()
    {
        GameObject.Destroy(m_player);
    }

    internal void OnMoveInput(Vector2 movement)
    {
        m_player.transform.position += new Vector3(movement.x, movement.y) * Time.deltaTime;
    }

    internal void OnLookGamePadInput(Vector2 movement)
    {
        var look = new Vector3(movement.x, movement.y, 0).normalized;
        var rotation = Quaternion.LookRotation(Vector3.forward, look);
        m_player.transform.rotation = rotation;

        m_debugLookVectorTarget.transform.position = m_player.transform.position + look;
    }

    internal void OnLookMouseInput(Vector2 movement, Vector3 mousePosition)
    {
        var look = (mousePosition - m_player.transform.position).normalized;
        var rotation = Quaternion.LookRotation(Vector3.forward, look);
        m_player.transform.rotation = rotation;

        m_debugLookVectorTarget.transform.position = mousePosition;
    }

    private void M_playerInput_OnLook(Vector2 lookDirection)
    {
    }

}
