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

        Entities.ForEach((ref Cat cat,
            in SleepAction sleepAct) =>
        {
            cat.tiredness = math.clamp(
                cat.tiredness - sleepAct.tirednessRecoverPerSecond * deltaTime, 0f, 100f);
        }).ScheduleParallel();
    }
}
