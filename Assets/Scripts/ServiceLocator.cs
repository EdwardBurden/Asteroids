using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ServiceLocator
{
    private static Dictionary<Type, ServiceInstance> m_lookup = new();

    public static void RegisterService<T>(T instance) where T : IService
    {
        m_lookup.Add(typeof(T), new ServiceInstance() { Instance = instance });

    }

    public static void RegisterService<T>(params object[] args) where T : IService
    {
        var service = new ServiceInstance();
        service.Instance = (T)Activator.CreateInstance(typeof(T), args);
        m_lookup.Add(typeof(T), service);
    }

    public static void UnRegisterService<T>() where T : IService
    {

        var type = typeof(T);
        m_lookup.Remove(type); //todo
    }

    public static T GetService<T>() where T : IService
    {
        var type = typeof(T);
        m_lookup.TryGetValue(type, out var instance); //todo
        T t = (T)instance.Instance;
        return t;
    }

    internal static void Update()
    {
      foreach (var service in m_lookup.Values)
        {

            service.Instance.Update();
        }
    }
}

public class ServiceInstance
{
    public IService Instance;
}
