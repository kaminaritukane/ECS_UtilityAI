using Unity.Entities;
using Unity.Jobs;

[DisableAutoCreation]
[UpdateInGroup(typeof(AISystemGroup))]
[UpdateAfter(typeof(ActionScoreSystem))]
public class ActionSelectionSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem endSimulationEcbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        // Choose action base on the highest score
        var ecb = endSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();
        Entities.ForEach((Entity entity,
            int entityInQueryIndex,
            ref Decision decision,
            in EatScorer eatScore,
            in SleepScorer sleepScore,
            in PlayScorer playScore) =>
        {
            float highestScore = 0.0f;
            ActionType actionToDo = ActionType.Play;
            if (eatScore.score > highestScore)
            {
                highestScore = eatScore.score;
                actionToDo = ActionType.Eat;
            }
            if (sleepScore.score > highestScore)
            {
                highestScore = sleepScore.score;
                actionToDo = ActionType.Sleep;
            }
            if (playScore.score > highestScore)
            {
                highestScore = playScore.score;
                actionToDo = ActionType.Play;
            }

            if (decision.action != actionToDo)
            {
                decision.action = actionToDo;

                switch (actionToDo)
                {
                    case ActionType.Eat:
                        ecb.RemoveComponent<SleepAction>(entityInQueryIndex, entity);
                        ecb.RemoveComponent<PlayAction>(entityInQueryIndex, entity);
                        ecb.AddComponent<EatAction>(entityInQueryIndex, entity);
                        ecb.SetComponent(entityInQueryIndex, entity, new EatAction()
                        {
                            hungerRecoverPerSecond = 5.0f,
                            tirednessCostPerSecond = 2.0f
                        });
                        break;
                    case ActionType.Sleep:
                        ecb.RemoveComponent<EatAction>(entityInQueryIndex, entity);
                        ecb.RemoveComponent<PlayAction>(entityInQueryIndex, entity);
                        ecb.AddComponent<SleepAction>(entityInQueryIndex, entity);
                        ecb.SetComponent(entityInQueryIndex, entity, new SleepAction()
                        {
                            tirednessRecoverPerSecond = 3.0f,
                            hungerCostPerSecond = 0.5f
                        });
                        break;
                    case ActionType.Play:
                        ecb.RemoveComponent<EatAction>(entityInQueryIndex, entity);
                        ecb.RemoveComponent<SleepAction>(entityInQueryIndex, entity);
                        ecb.AddComponent<PlayAction>(entityInQueryIndex, entity);
                        ecb.SetComponent(entityInQueryIndex, entity, new PlayAction()
                        {
                            hungerCostPerSecond = 2.0f,
                            tirednessCostPerSecond = 4.0f
                        });
                        break;
                }
            }
        }).ScheduleParallel();

        endSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
