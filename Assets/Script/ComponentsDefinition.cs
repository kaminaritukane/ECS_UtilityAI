using Unity.Entities;

enum ActionType
{
    Eat,
    Sleep,
    Play,
}

struct EatAction : IComponentData { }
struct SleepAction : IComponentData { }
struct PlayAction : IComponentData { }

struct EatScore : IComponentData { public float score; }
struct SleepScore : IComponentData { public float score; }
struct PlayScore : IComponentData { public float score; }

struct Cat : IComponentData
{
    public int hungry;// 0: not hungry, 100: hungry to death
    public int tiredness;// 0: not tired, 100: tired to death
    public ActionType action;// current action to perform
}