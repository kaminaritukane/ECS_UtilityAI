using System;
using Unity.Entities;
using Unity.Jobs;

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
            ref Cat cat,
            in EatScore eatScore,
            //in SleepScore sleepScore,
            in PlayScore playScore) =>
        {
            float highestScore = 0.0f;
            ActionType actionToDo = ActionType.Play;
            if (eatScore.score > highestScore)
            {
                highestScore = eatScore.score;
                actionToDo = ActionType.Eat;
            }
            //if ( sleepScore.score > highestScore )
            //{
            //    highestScore = sleepScore.score;
            //    actionToDo = ActionType.Sleep;
            //}
            if (playScore.score > highestScore)
            {
                highestScore = playScore.score;
                actionToDo = ActionType.Play;
            }

            if (cat.action != actionToDo)
            {
                cat.action = actionToDo;

                switch (actionToDo)
                {
                    case ActionType.Eat:
                        //ecb.RemoveComponent<SleepAction>(entityInQueryIndex, entity);
                        ecb.RemoveComponent<PlayAction>(entityInQueryIndex, entity);
                        ecb.AddComponent<EatAction>(entityInQueryIndex, entity);
                        ecb.SetComponent(entityInQueryIndex, entity, new EatAction()
                        {
                            hungerRecoverPerSecond = 5.0f
                        });
                        break;
                    //case ActionType.Sleep:
                    //    ecb.RemoveComponent<EatAction>(entityInQueryIndex, entity);
                    //    ecb.RemoveComponent<PlayAction>(entityInQueryIndex, entity);
                    //    ecb.AddComponent<SleepAction>(entityInQueryIndex, entity);
                    //    break;
                    case ActionType.Play:
                        ecb.RemoveComponent<EatAction>(entityInQueryIndex, entity);
                        //ecb.RemoveComponent<SleepAction>(entityInQueryIndex, entity);
                        ecb.AddComponent<PlayAction>(entityInQueryIndex, entity);
                        ecb.SetComponent(entityInQueryIndex, entity, new PlayAction()
                        {
                            hungerCostPerSecond = 5.0f
                        });
                        break;
                }
            }
        }).ScheduleParallel();

        endSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
