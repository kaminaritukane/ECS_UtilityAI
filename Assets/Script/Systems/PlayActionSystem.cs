using Unity.Entities;
using Unity.Jobs;

public class PlayActionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Cat cat,
            in PlayAction playAct) =>
        {
            cat.hunger += playAct.hungerCostPerSecond * deltaTime;
            cat.tiredness += playAct.tirednessCostPerSecond * deltaTime;
            //Debug.Log($"Playing hunger: {cat.hunger}");
        }).ScheduleParallel();
    }
}
