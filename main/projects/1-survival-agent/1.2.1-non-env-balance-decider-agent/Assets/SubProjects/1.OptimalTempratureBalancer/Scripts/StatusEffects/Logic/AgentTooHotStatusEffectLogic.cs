using UnityEngine;

public class AgentTooHotStatusEffectLogic : BaseStatusEffectLogic {

    // Properties
    private AgentTooHotStatusEffectData _tooHotData;
    private OptimalTempratureBalancerAgentStats _agentStats;

    // Overriden Methods

    public override void Initialize(AgentStats targetStats, BaseStatusEffectData data) {
        base.Initialize(targetStats, data);
        _tooHotData = data as AgentTooHotStatusEffectData;
        _agentStats = _targetStats as OptimalTempratureBalancerAgentStats;
    }
    public override void OnEffectApplied() {
    }

    public override void OnEffectUpdate(float deltaTime) {
        base.OnEffectUpdate(deltaTime);
        _agentStats.Temprature.CurrentValue += deltaTime * _tooHotData.tempratureIncrementPerTick;
    }

    public override void OnEffectRemoved() {
    }
}
