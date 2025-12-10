using System;
using System.Collections;
using UnityEngine;


public class Obstacle : PooledGameObject
{
    [SerializeField] private Rotator m_rotator;

    private WaitForSeconds m_aliveTime;
    private Vector3 direction;

    public void Setup(ObstacleData data)
    {
        //reset position. better to do in obstacle /level code
        m_aliveTime = new WaitForSeconds(data.AliveTimeMS / 1000);
        StartCoroutine(CountDownLife());
        direction = new Vector3( UnityEngine.Random.Range(1f, -1f), UnityEngine.Random.Range(1f, -1f) , 0);
    }



    private IEnumerator CountDownLife() 
    {
        yield return m_aliveTime;
        if (this.gameObject != null || this.isActiveAndEnabled) 
        {
            Destroy();
        }
    }

    private void Update()
    {
        this.transform.position +=  direction*Time.deltaTime;
    }
}
