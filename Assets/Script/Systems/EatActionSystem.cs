﻿using Unity.Entities;
using Unity.Jobs;

public class EatActionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Cat cat,
            in EatAction eatAct) =>
        {
            cat.hunger -= eatAct.hungerRecoverPerSecond * deltaTime;
        }).ScheduleParallel();
    }
}
