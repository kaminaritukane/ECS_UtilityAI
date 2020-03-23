using Unity.Entities;
using UnityEngine;

public class CatEntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Cat() { 
            hungry = 0, 
            tiredness = 0, 
            action = ActionType.Play
        });
        dstManager.AddComponentData(entity, new EatScore() { score = 0 }); ;
        dstManager.AddComponentData(entity, new SleepScore() { score = 0 });
        dstManager.AddComponentData(entity, new PlayScore() { score = 0 });
    }
}
