using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[DisableAutoCreation]
[UpdateInGroup(typeof(ActionSystemGroup))]
public class SleepActionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Tiredness tired,
            ref Hungriness hunger,
            in SleepAction sleepAct) =>
        {
            // recover tiredness
            tired.value = math.clamp(
                tired.value - sleepAct.tirednessRecoverPerSecond * deltaTime, 0f, 100f);

            // sleep still get hungry slowly
            hunger.value = math.clamp(
                hunger.value + sleepAct.hungerCostPerSecond * deltaTime, 0f, 100f);
        }).ScheduleParallel();
    }
}
