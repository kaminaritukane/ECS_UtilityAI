using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[DisableAutoCreation]
[UpdateInGroup(typeof(AISystemGroup))]
public class ActionScoreSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //>> Calculate scores
        // TODO: To emplement the curves to the scorer of each action
        Entities.ForEach((ref EatScorer eatScore, in Cat cat) =>
        {
            if (cat.action == ActionType.Eat)
            {
                // once it starts to eat, it will not stop until it's full
                eatScore.score = cat.hunger <= float.Epsilon ? 0f : 1f;
            }
            else
            {
                var input = math.clamp(cat.hunger * 0.01f, 0f, 1f);
                eatScore.score = ResponseCurve.Exponentional(input, 2f);
            }
        }).ScheduleParallel();

        Entities.ForEach((ref SleepScorer sleepScore, in Cat cat) =>
        {
            if (cat.action == ActionType.Sleep)
            {
                // once it starts to sleep, it will not awake until it have enough rest
                sleepScore.score = cat.tiredness <= float.Epsilon ? 0f : 1f;
            }
            else
            {
                var input = math.clamp(cat.tiredness * 0.01f, 0f, 1f);
                sleepScore.score = ResponseCurve.RaiseFastToSlow(input, 4);
            }
        }).ScheduleParallel();

        Entities.ForEach((ref PlayScorer playScore, in Cat cat) =>
        {
            // The play scorer has two considerations
            // The cat will play when it feels neigher hungry nor tired
            // Let's say it hate tired more(love to sleep), so the sleep consideration get more weight
            // sleep weight: 0.6, eat weight: 0.4

            var eatConcern = ResponseCurve.Exponentional(math.clamp(cat.hunger * 0.01f, 0f, 1f));
            var sleepConcern = ResponseCurve.RaiseFastToSlow(math.clamp(cat.tiredness * 0.01f, 0f, 1f));
            
            var concernBothersPlaying = sleepConcern * 0.6f + eatConcern * 0.4f;

            playScore.score = math.clamp(1f - concernBothersPlaying, 0f, 1f);
        }).ScheduleParallel();
        //<<

        this.CompleteDependency();
    }
}
