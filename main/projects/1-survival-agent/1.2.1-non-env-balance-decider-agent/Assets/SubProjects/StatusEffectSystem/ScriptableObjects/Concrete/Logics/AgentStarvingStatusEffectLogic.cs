using UnityEngine;

public class AgentStarvingStatusEffectLogic : BaseStatusEffectLogic {

    // Properties
    private AgentStarvingStatusEffectData _starvingData;

    // Overriden Methods
    public override void Initialize(AgentStats targetStats, BaseStatusEffectData data) {
        base.Initialize(targetStats, data);
        _starvingData = data as AgentStarvingStatusEffectData;
    }

    public override void OnEffectApplied() {
        
    }

    public override void OnEffectUpdate(float deltaTime) {
        base.OnEffectUpdate(deltaTime);
        if (IsExpired) return;
        _targetStats.Health.CurrentValue -= _starvingData.starvationPerTick * deltaTime;
    }

    public override void OnEffectRemoved() {
    }
}
