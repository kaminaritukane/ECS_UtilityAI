﻿using Unity.Entities;
using UnityEngine;

public class CatEntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Cat() { 
            hunger = 0, 
            //tiredness = 0, 
            action = ActionType.Null
        });
        dstManager.AddComponentData(entity, new EatScore()); ;
        //dstManager.AddComponentData(entity, new SleepScore() { score = 0 });
        dstManager.AddComponentData(entity, new PlayScore());
    }
}