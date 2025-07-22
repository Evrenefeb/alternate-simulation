using UnityEngine;

public class AgentWindyStatusEffectLogic : BaseStatusEffectLogic {

    // Properties
    private AgentWindyStatusEffectData _windyData;
    private OptimalTempratureBalancerAgentStats _agentStats;

    // Overriden Methods

    public override void Initialize(AgentStats targetStats, BaseStatusEffectData data) {
        base.Initialize(targetStats, data);
        _windyData = data as AgentWindyStatusEffectData;
        _agentStats = _targetStats as OptimalTempratureBalancerAgentStats;
    }
    public override void OnEffectApplied() {
        _agentStats.MovementStep.CurrentValue -= _windyData.intensity;
    }

    public override void OnEffectUpdate(float deltaTime) {
        base.OnEffectUpdate(deltaTime);
    }

    public override void OnEffectRemoved() {
        _agentStats.MovementStep.CurrentValue += _windyData.intensity;
    }
}
