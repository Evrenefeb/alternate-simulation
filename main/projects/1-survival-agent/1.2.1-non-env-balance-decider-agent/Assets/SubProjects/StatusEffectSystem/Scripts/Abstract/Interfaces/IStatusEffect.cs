using UnityEngine;

public interface IStatusEffect {
    public uint ID { get; }
    public string EffectName { get; }
    public float Duration { get; }
    public float RemainingTime { get; }
    void Initialize(AgentStats targetStats, BaseStatusEffectData data);
    void OnEffectApplied();
    void OnEffectUpdate(float deltaTime);
    void OnEffectRemoved();
    void OnEffectReset();
    bool IsBaseReferencesNull();
}

