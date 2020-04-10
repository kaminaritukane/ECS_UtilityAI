using Unity.Entities;
using Unity.Jobs;

public class SleepActionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Cat cat,
            in SleepAction sleepAct) =>
        {
            cat.tiredness -= sleepAct.tirednessRecoverPerSecond * deltaTime;
        }).ScheduleParallel();
    }
}
