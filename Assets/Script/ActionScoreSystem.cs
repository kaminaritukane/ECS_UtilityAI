using System;
using Unity.Entities;
using Unity.Jobs;

public class ActionScoreSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem endSimulationEcbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        //>> Calculate scores
        Entities.ForEach((ref EatScore eatScore, in Cat cat) =>
        {
            eatScore.score = cat.hungry;
        }).ScheduleParallel();

        Entities.ForEach((ref SleepScore sleepScore, in Cat cat) =>
        {
            sleepScore.score = cat.tiredness;
        }).ScheduleParallel();

        Entities.ForEach((ref PlayScore playScore, in Cat cat) =>
        {
            if (cat.hungry <= 50 && cat.tiredness <= 50)
            {
                playScore.score = 100;
            }
            else
            {
                playScore.score = 0;
            }
        }).ScheduleParallel();
        //<<

        // Choose action base on the highest score
        var ecb = endSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();
        Entities.ForEach((Entity entity,
            int entityInQueryIndex,
            ref Cat cat,
            in EatScore eatScore, 
            in SleepScore sleepScore,
            in PlayScore playScore) =>
        {
            float highestScore = 0.0f;
            ActionType actionToDo = ActionType.Play;
            if ( eatScore.score > highestScore )
            {
                highestScore = eatScore.score;
                actionToDo = ActionType.Eat;
            }
            if ( sleepScore.score > highestScore )
            {
                highestScore = sleepScore.score;
                actionToDo = ActionType.Sleep;
            }
            if ( playScore.score > highestScore )
            {
                highestScore = playScore.score;
                actionToDo = ActionType.Play;
            }

            switch(actionToDo)
            {
                case ActionType.Eat:
                    ecb.RemoveComponent<SleepAction>(entityInQueryIndex, entity);
                    ecb.RemoveComponent<PlayAction>(entityInQueryIndex, entity);
                    ecb.AddComponent<EatAction>(entityInQueryIndex, entity);
                    break;
                case ActionType.Sleep:
                    ecb.RemoveComponent<EatAction>(entityInQueryIndex, entity);
                    ecb.RemoveComponent<PlayAction>(entityInQueryIndex, entity);
                    ecb.AddComponent<SleepAction>(entityInQueryIndex, entity);
                    break;
                case ActionType.Play:
                    ecb.RemoveComponent<EatAction>(entityInQueryIndex, entity);
                    ecb.RemoveComponent<SleepAction>(entityInQueryIndex, entity);
                    ecb.AddComponent<PlayAction>(entityInQueryIndex, entity);
                    break;
            }
        }).ScheduleParallel();
    }
}
