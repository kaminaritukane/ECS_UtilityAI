using System;
using Unity.Entities;
using Unity.Jobs;

public class ActionScoreSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //>> Calculate scores
        // TODO: To emplement the curves to the scorer of each action
        Entities.ForEach((ref EatScore eatScore, in Cat cat) =>
        {
            if ( cat.action == ActionType.Eat )
            {
                eatScore.score = cat.hunger <= float.Epsilon ? 0 : 100;
            }
            else
            {
                eatScore.score = cat.hunger;
            }
        }).ScheduleParallel();

        //Entities.ForEach((ref SleepScore sleepScore, in Cat cat) =>
        //{
        //    sleepScore.score = cat.tiredness;
        //}).ScheduleParallel();

        Entities.ForEach((ref PlayScore playScore, in Cat cat) =>
        {
            playScore.score = 100 - cat.hunger;
        }).ScheduleParallel();
        //<<

        this.CompleteDependency();
    }
}
