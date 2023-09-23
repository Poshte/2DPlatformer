using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    
    private ServiceLocator()
    {

    }
    private readonly Dictionary<string, IGameService> services = new Dictionary<string, IGameService>();

    public static ServiceLocator Instance { get; private set; }


    public static void Initialize()
    {
        Instance = new ServiceLocator();
    }

    public T Get<T>() where T : IGameService
    {
        var key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }

        return (T)services[key];
    }

    public void Register<T>(T service) where T : IGameService
    {
        var key = typeof(T).Name;
        if (services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to register service of type {key} which is already registered with the {GetType().Name}");
            return;
        }

        services.Add(key, service);
    }

    public void Unregister<T>() where T : IGameService
    {
        var key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}");
            return;
        }

        services.Remove(key);
    }
}
