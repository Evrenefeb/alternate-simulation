using UnityEngine;

public class AgentSlownessStatusEffectLogic : BaseStatusEffectLogic {

    // Properties
    private AgentSlownessStatusEffectData _slownessData;
    private float _tickTimer;

    // Overriden Methods
    public override void Initialize(AgentStats targetStats, BaseStatusEffectData data) {
        base.Initialize(targetStats, data);
        _slownessData = data as AgentSlownessStatusEffectData;
    }

    public override void OnEffectApplied() {
        float slownessFactor = _slownessData.SlownessFactor;
        switch (_slownessData.slownessType) {
            case AgentSlownessStatusEffectData.SlownessType.Subtract:
                _targetStats.MovementSpeed.MaxValue -= slownessFactor;
                break;
            case AgentSlownessStatusEffectData.SlownessType.Multiply:
                slownessFactor = Mathf.Clamp01(slownessFactor);
                _targetStats.MovementSpeed.MaxValue *= slownessFactor;
                break;
            default:
                break;
        }
    }

    public override void OnEffectUpdate(float deltaTime) {
        base.OnEffectUpdate(deltaTime);


    }

    public override void OnEffectRemoved() {
        float revertedFactor = _slownessData.SlownessFactor;
        switch (_slownessData.slownessType) {
            case AgentSlownessStatusEffectData.SlownessType.Subtract:
                _targetStats.MovementSpeed.MaxValue += revertedFactor;
                break;
            case AgentSlownessStatusEffectData.SlownessType.Multiply:
                revertedFactor = Mathf.Clamp01(revertedFactor);
                _targetStats.MovementSpeed.MaxValue /= revertedFactor;
                break;
            default:
                break;
        }
    }
}
