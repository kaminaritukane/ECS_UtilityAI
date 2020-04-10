using Unity.Entities;
using UnityEngine;

enum ActionType
{
    Null,
    Eat,
    Sleep,
    Play,
}

struct EatAction : IComponentData 
{
    public float hungerRecoverPerSecond;
}
struct SleepAction : IComponentData 
{
    public float tirednessRecoverPerSecond;
}
struct PlayAction : IComponentData 
{
    public float hungerCostPerSecond;
    public float tirednessCostPerSecond;
}

struct EatScorer : IComponentData { public float score; }
struct SleepScorer : IComponentData { public float score; }
struct PlayScorer : IComponentData { public float score; }

struct Cat : IComponentData
{
    public float hunger;// 0: not hungry, 100: hungry to death
    public float tiredness;// 0: not tired, 100: tired to death
    public ActionType action;// current action to perform
}