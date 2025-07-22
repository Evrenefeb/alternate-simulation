using UnityEngine;

public abstract class BaseStatusEffectLogic : IStatusEffect {

    // Properties
    protected AgentStats _targetStats;
    protected BaseStatusEffectData _effectData;
    private bool _isExpired;
    public bool IsExpired => _isExpired;

    public uint ID => _effectData.ID;
    public string EffectName => _effectData.EffectName;

    public float Duration => _effectData.Duration;

    public float RemainingTime { get; protected set; }


    // Interface Methods

    public virtual void Initialize(AgentStats targetStats, BaseStatusEffectData data) {
        _targetStats = targetStats;
        _effectData = data;
        RemainingTime = Duration;
        _isExpired = false;
    }

    public abstract void OnEffectApplied();
    public virtual void OnEffectUpdate(float deltaTime) {
        if (IsBaseReferencesNull()) return;
        if (RemainingTime > 0) {
            RemainingTime -= deltaTime;
        }
        else if (!_isExpired) {
            _isExpired = true;
        }
    }

    public abstract void OnEffectRemoved();

    public virtual void OnEffectReset() {
        RemainingTime = Duration;
    }

    public virtual bool IsBaseReferencesNull() {
        if(_targetStats == null ||_effectData == null) return true;
        return false;
    }
}
