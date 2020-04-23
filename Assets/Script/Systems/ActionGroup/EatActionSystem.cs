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

        Entities.ForEach((ref Cat cat,
            in EatAction eatAct) =>
        {
            cat.hunger = math.clamp(
                cat.hunger - eatAct.hungerRecoverPerSecond * deltaTime, 0f, 100f);
        }).ScheduleParallel();
    }
}
