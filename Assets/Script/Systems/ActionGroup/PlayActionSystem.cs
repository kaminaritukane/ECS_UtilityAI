using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[DisableAutoCreation]
[UpdateInGroup(typeof(ActionSystemGroup))]
public class PlayActionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Cat cat,
            in PlayAction playAct) =>
        {
            cat.hunger = math.clamp(
                cat.hunger + playAct.hungerCostPerSecond * deltaTime, 0f, 100f);
            cat.tiredness = math.clamp(
                cat.tiredness + playAct.tirednessCostPerSecond * deltaTime, 0f, 100f);
        }).ScheduleParallel();
    }
}
