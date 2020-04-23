using Unity.Entities;
using UnityEngine;

public class AISystemGroup : ComponentSystemGroup
{
    private const float AI_UPDATE_FREQUANCY = 1f;// times per seconds
    private float m_aiUpdateInterval = 1f / AI_UPDATE_FREQUANCY;
    private float m_aiUpdateCD = 0f;

    protected override void OnCreate()
    {
        base.OnCreate();

        AddSystemToUpdateList(World.CreateSystem<ActionScoreSystem>());
        AddSystemToUpdateList(World.CreateSystem<ActionSelectionSystem>());
    }

    protected override void OnUpdate()
    {
        // AI system should update in a lower frequancy
        if ( m_aiUpdateCD <= 0.0f )
        {
            m_aiUpdateCD += m_aiUpdateInterval;

            base.OnUpdate();
        }

        m_aiUpdateCD -= Time.DeltaTime;
    }
}
