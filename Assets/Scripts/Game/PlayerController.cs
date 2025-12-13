using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour, IGameSystem
{
    [SerializeField] private Transform m_bulletPoolTransform;
    [SerializeField] private Transform m_playerRoot;
    [SerializeField] private GameObject m_debugLookVectorTarget;
    private Player m_player;
    private PlayerData m_playerData;
    private BulletData m_bulletData;

    private GameObjectPool<Bullet> m_bulletPool = new GameObjectPool<Bullet>();

    public void Startup()
    {
        m_playerData = App.Instance.GameData.PlayerData;
        m_bulletData = m_playerData.Bullets;
        m_player = GameObject.Instantiate(m_playerData.Prefab, m_playerRoot);
        Game.Instance.InputManager.OnMove += OnMoveInput;
        Game.Instance.InputManager.OnLook_GamePad += OnLookGamePadInput;
        Game.Instance.InputManager.OnLook_Mouse += OnLookMouseInput;
        Game.Instance.InputManager.OnShootPressed +=Shoot;

        m_bulletPool.Warm(m_bulletData.Prefab, 12, m_bulletPoolTransform);
    }

    private void Shoot()
    {
     var bullet =  m_bulletPool.Allocate();
        bullet.transform.position = m_player.BulletRoot;


        var direction = (m_debugLookVectorTarget.transform.position - bullet.transform.position).normalized;
        bullet.Move(direction, m_bulletData);
        bullet.gameObject.SetActive(true);
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
