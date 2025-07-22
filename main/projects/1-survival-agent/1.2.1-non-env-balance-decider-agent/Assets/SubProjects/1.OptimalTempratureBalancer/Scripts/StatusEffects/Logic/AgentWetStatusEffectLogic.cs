using UnityEngine;

public class AgentWetStatusEffectLogic : BaseStatusEffectLogic {

    // Properties
    private AgentWetStatusEffectData _wetData;
    private OptimalTempratureBalancerAgentStats _agentStats;

    // Overriden Methods

    public override void Initialize(AgentStats targetStats, BaseStatusEffectData data) {
        base.Initialize(targetStats, data);
        _wetData = data as AgentWetStatusEffectData;
        _agentStats = _targetStats as OptimalTempratureBalancerAgentStats;
    }
    public override void OnEffectApplied() {
        var temp = _agentStats.Temprature.CurrentValue;
        if(temp > _wetData.wetHotThreshold) {
            // cool off (Gain reward)

        } else if (temp < _wetData.wetColdThreshold) {
            // freezing (Loose Reward)

        }
        else {
            return;
        }
    }

    public override void OnEffectUpdate(float deltaTime) {
        base.OnEffectUpdate(deltaTime);
        _agentStats.Temprature.CurrentValue -= deltaTime * _wetData.coolingRate;
    }

    public override void OnEffectRemoved() {
    }
}
