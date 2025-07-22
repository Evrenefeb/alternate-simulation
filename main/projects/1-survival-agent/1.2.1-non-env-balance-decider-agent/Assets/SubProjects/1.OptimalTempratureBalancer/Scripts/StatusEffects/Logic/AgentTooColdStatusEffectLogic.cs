using UnityEngine;

public class AgentTooColdStatusEffectLogic : BaseStatusEffectLogic {

    // Properties
    private AgentTooColdStatusEffectData _tooColdData;
    private OptimalTempratureBalancerAgentStats _agentStats;

    // Overriden Methods

    public override void Initialize(AgentStats targetStats, BaseStatusEffectData data) {
        base.Initialize(targetStats, data);
        _tooColdData = data as AgentTooColdStatusEffectData;
        _agentStats = _targetStats as OptimalTempratureBalancerAgentStats;
    }
    public override void OnEffectApplied() {
    }

    public override void OnEffectUpdate(float deltaTime) {
        base.OnEffectUpdate(deltaTime);
        _agentStats.Temprature.CurrentValue -= deltaTime * _tooColdData.tempratureDecrementPerTick;
    }

    public override void OnEffectRemoved() {
    }
}
