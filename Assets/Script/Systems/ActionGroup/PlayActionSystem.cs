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

        Entities.ForEach((ref Hungriness hunger,
            ref Tiredness tired,
            in PlayAction playAct) =>
        {
            hunger.value = math.clamp(
                hunger.value + playAct.hungerCostPerSecond * deltaTime, 0f, 100f);
            tired.value = math.clamp(
                tired.value + playAct.tirednessCostPerSecond * deltaTime, 0f, 100f);
        }).ScheduleParallel();
    }
}
