using Unity.Entities;
using UnityEngine;

public class CatEntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent(entity, ComponentType.ReadOnly<Cat>());
        dstManager.AddComponentData(entity, new Hungriness { value = 0 });
        dstManager.AddComponentData(entity, new Tiredness { value = 0 });
        dstManager.AddComponentData(entity, new Decision { action = ActionType.Null });

        dstManager.AddComponent(entity, ComponentType.ReadWrite<EatScorer>()); ;
        dstManager.AddComponent(entity, ComponentType.ReadWrite<SleepScorer>());
        dstManager.AddComponent(entity, ComponentType.ReadWrite<PlayScorer>());
    }
}
