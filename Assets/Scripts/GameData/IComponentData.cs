using System;
using UnityEngine;

public interface IComponentData
{
    bool Validate(BaseTest prefab);

    void Initialse(BaseTest prefab);
}


[Serializable]
public class HealthData : IComponentData
{
    public void Initialse(BaseTest prefab)
    {
        prefab.GetComponent<HealthComponent>().Setup(/*HERE*/); //would need cached list too
    }

    public bool Validate(BaseTest prefab)
    {
       return  prefab.GetComponent<HealthComponent>() != null;
    }
}



[CreateAssetMenu(fileName = "TTTESt", menuName = "GameData/Test")]
public class BaseTestData : SpaceObjectData<Player>
{
    public IComponentData[] Components; //requires drawer for making , culd limit by having a enum for each class and restricting 
    public BaseTest prefab;

    private void OnValidate()
    {
        foreach (var item in Components)
        {
            item.Validate(prefab);
        }
    }

    public void SetupData(BaseTest baseTest) {
        foreach (var item in Components)
        {
            item.Initialse(baseTest);
        }

    }
}


public class BaseTest :MonoBehaviour
{
    // has list of components cached here to save using getcomponent
    //would be replacement for 


    public void Setup(BaseTestData baseTestData)
    {
        baseTestData.SetupData(this);


    }

}