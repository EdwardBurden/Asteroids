using UnityEngine;

public class PlayerController : MonoBehaviour, IGameSystem
{
    [SerializeField] private Transform m_bulletPoolTransform;
    [SerializeField] private Transform m_playerRoot;
    [SerializeField] private GameObject m_debugLookVectorTarget;
    private Player m_player;
    private BulletData m_bulletData;

    private GameObjectPool<Bullet> m_bulletPool = new GameObjectPool<Bullet>();
    public PlayerData CurrentPlayerData { get; private set; }

    public void Startup()
    {
        Game.Instance.InputManager.OnMove += OnMoveInput;
        Game.Instance.InputManager.OnLook_GamePad += OnLookGamePadInput;
        Game.Instance.InputManager.OnLook_Mouse += OnLookMouseInput;
        Game.Instance.InputManager.OnShootPressed += Shoot;
    }

    public void ReadyPlayer() 
    {
        var playerIndex = UnityEngine.Random.Range(0, App.Instance.GameData.PlayerData.Length);
        CurrentPlayerData = App.Instance.GameData.PlayerData[playerIndex];
        m_bulletData = CurrentPlayerData.Bullets;
        m_bulletPool.Clear();
        m_bulletPool.Warm(m_bulletData.Prefab, 12, m_bulletPoolTransform);
        if (m_player != null)
        {
            GameObject.Destroy(m_player.gameObject);
        }
        m_player = GameObject.Instantiate(CurrentPlayerData.Prefab, m_playerRoot);
        m_player.Setup(CurrentPlayerData);
        m_player.Damaged += OnDamageTaken;
    }

    private void Shoot()
    {
        var bullet = m_bulletPool.Allocate();
        bullet.transform.position = m_player.BulletRoot;
        var direction = (m_debugLookVectorTarget.transform.position - bullet.transform.position).normalized;
        bullet.Setup(direction, m_bulletData);
        bullet.gameObject.SetActive(true);
    }

    private void OnDamageTaken(int amount)
    {
        Game.Instance.PlayerTakenDamage(amount);
    }

    public void ShutDown()
    {
        GameObject.Destroy(m_player);
        m_bulletPool.Clear();
    }

    internal void OnMoveInput(Vector2 movement)
    {
        m_player.transform.position += new Vector3(movement.x, movement.y) * Time.deltaTime * CurrentPlayerData.Speed;
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
