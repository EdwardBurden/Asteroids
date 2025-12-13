using UnityEngine;

public class Player : MonoBehaviour
{
 [SerializeField]   private Transform m_bulletRoot;
    public Vector3 BulletRoot => m_bulletRoot.position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
