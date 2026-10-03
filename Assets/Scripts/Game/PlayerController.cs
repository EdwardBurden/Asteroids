using UnityEngine;

public class PlayerController : MonoBehaviour, IGameSystem
{
    [SerializeField] private Transform m_bulletPoolTransform;
    [SerializeField] private Transform m_playerRoot;
    [SerializeField] private GameObject m_debugLookVectorTarget;

    private Player m_player;
    private BulletData m_bulletData;
    private float m_reloadTimer;
    private float m_reloadTime;   
    
    private GameObjectPool<Bullet> m_bulletPool = new GameObjectPool<Bullet>();
    public PlayerData CurrentPlayerData { get; private set; }
    public Vector3 PlayerPosition => m_player.transform.position;
    private bool CanShoot => m_reloadTimer > m_reloadTime;
    public void Startup()
    {
        Debug.Log($"{nameof(PlayerController)} Startup");
        var playerIndex = UnityEngine.Random.Range(0, App.Instance.GameData.PlayerData.Length);
        CurrentPlayerData = App.Instance.GameData.PlayerData[playerIndex];
        m_bulletData = CurrentPlayerData.Bullets;
        if (m_bulletData != null)
        {
            m_bulletPool.Clear();
            m_bulletPool.Warm(m_bulletData.Prefab, 12, m_bulletPoolTransform);
            m_reloadTime = m_bulletData.ReloadTime ;
        }

        if (m_player != null)
        {
            GameObject.Destroy(m_player.gameObject);
        }
        m_player = GameObject.Instantiate(CurrentPlayerData.Prefab, m_playerRoot);
        m_player.gameObject.SetActive(false);
        m_player.Setup(CurrentPlayerData);
        m_player.Damaged += OnDamageTaken;
        m_player.Healed += OnHealed;
        Game.Instance.InputManager.OnMove += OnMoveInput;
        Game.Instance.InputManager.OnLook_GamePad += OnLookGamePadInput;
        Game.Instance.InputManager.OnLook_Mouse += OnLookMouseInput;
        Game.Instance.InputManager.OnShootPressed += Shoot;
    }

    public void ShutDown()
    {
        Debug.Log($"{nameof(PlayerController)} ShutDown");
        GameObject.Destroy(m_player);
        m_bulletPool.Clear();
    }

    public void ReadyPlayer() 
    {
        m_player.gameObject.SetActive(true);
    }

    private void Shoot()
    {
        if (m_bulletData != null && CanShoot)
        {
            //todo use bullet reload time here
            var bullet = m_bulletPool.Allocate();
            bullet.transform.position = m_player.BulletRoot;
            bullet.Setup(m_player.transform.up, m_bulletData);
            bullet.gameObject.SetActive(true);
            m_reloadTimer = 0 ;
        }
    }

    private void Update()
    {
        m_reloadTimer += Time.deltaTime;
    }

    private void OnDamageTaken(int amount)
    {
        Game.Instance.PlayerTakenDamage(amount);
    }

    private void OnHealed(int amount)
    {
        Game.Instance.PlayerGainedHealth(amount);
    }

    internal void OnMoveInput(Vector2 movement)
    {
        // if frozen or something do this, else normal move
        m_player.Move(movement);
    }

    internal void OnLookGamePadInput(Vector2 movement)
    {
        // todo should be moved into player input component like movement
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
}
