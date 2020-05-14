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
    public float tirednessCostPerSecond;// eat still get tired, but should slower than play
}
struct SleepAction : IComponentData 
{
    public float tirednessRecoverPerSecond;
    public float hungerCostPerSecond;// sleep still get hungry slowly
}
struct PlayAction : IComponentData 
{
    // the hunger cost and tiredness cost of play are faster than eat and sleep
    public float hungerCostPerSecond;
    public float tirednessCostPerSecond;
}

struct EatScorer : IComponentData { public float score; }
struct SleepScorer : IComponentData { public float score; }
struct PlayScorer : IComponentData { public float score; }

struct Cat : IComponentData
{
    // This component have no fields cause we just want it to be a flag
}

struct Hungriness : IComponentData
{
    public float value; // 0: not hungry, 100: hungry to death
}
struct Tiredness : IComponentData
{
    public float value; // 0: not tired, 100: tired to death
}
struct Decision : IComponentData
{
    public ActionType action; // current action to perform
}