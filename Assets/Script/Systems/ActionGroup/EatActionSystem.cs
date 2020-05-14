using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[DisableAutoCreation]
[UpdateInGroup(typeof(ActionSystemGroup))]
public class EatActionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Hungriness hunger,
            ref Tiredness tired,
            in EatAction eatAct) =>
        {
            // recover hungriness
            hunger.value = math.clamp(
                hunger.value - eatAct.hungerRecoverPerSecond * deltaTime, 0f, 100f);

            // eat still get tired, but should slower than play
            tired.value = math.clamp(
                tired.value + eatAct.tirednessCostPerSecond * deltaTime, 0f, 100f);
        }).ScheduleParallel();
    }
}
