using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public interface IPool
{
    public void Free(IPoolable poolable);
}

public class GameObjectPool<T> : IPool where T : PooledGameObject
{
    private readonly Stack<T> m_inactivePool = new Stack<T>();
    private readonly List<T> m_activePool = new List<T>();
    private bool m_allowResize;
    private Transform m_parent;
    private T m_prefab;
    private int m_size;
    private Action<T> m_deSpawnCallback;


    public void Warm(T prefab, int size, Transform parent, bool allowResize = true, Action<T> deSpawnCallback = null)
    {
        m_allowResize = allowResize;
        m_parent = parent;
        m_prefab = prefab;
        m_size = size;
        m_deSpawnCallback = deSpawnCallback;
        for (int i = 0; i < size; i++)
        {
            SpawnInternal();
        }
    }

    private void SpawnInternal()
    {
        var spawned = GameObject.Instantiate(m_prefab, m_parent);
        spawned.gameObject.SetActive(false);
        m_inactivePool.Push(spawned);
    }

    public T Allocate()
    {
        if (m_inactivePool.Count == 0 && m_allowResize)
        {
            SpawnInternal();
        }

        var prefab = m_inactivePool.Pop();
        m_activePool.Add(prefab);
        prefab.Spawn(this);
        return prefab;
    }


    public void Free(IPoolable activeObject)
    {
        m_activePool.Remove((T)activeObject);
        m_inactivePool.Push((T)activeObject);
        m_deSpawnCallback?.Invoke((T)activeObject);
    }

    public void Clear()
    {
        Utils.DestoryAllChildren(m_parent);
        m_activePool.Clear();
        m_inactivePool.Clear();
        m_deSpawnCallback = null;
    }
}

public interface IPoolable
{
    public void Spawn(IPool poolHandl);
}

public abstract class PooledGameObject : MonoBehaviour, IPoolable
{
    private IPool m_poolHandle;

    protected void Destroy()
    {
        m_poolHandle.Free(this);
        this.gameObject.SetActive(false);
        StopAllCoroutines(); //check later if needed
    }

    public void Spawn(IPool poolHandle)
    {
        m_poolHandle = poolHandle;
    }
}