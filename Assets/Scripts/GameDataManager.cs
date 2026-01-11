using UnityEngine;

public class GameDataManager : IService
{
    public GameDataManager(Entrypoint entrypoint) 
    {
        GameData = entrypoint.m_gameData;
    }

    public GameData GameData { get; internal set; }

    public void ShutDown()
    {
       // throw new System.NotImplementedException();
    }

    public void Startup()
    {
       // throw new System.NotImplementedException();
    }

    public void Update()
    {
        //throw new System.NotImplementedException();
    }
}
