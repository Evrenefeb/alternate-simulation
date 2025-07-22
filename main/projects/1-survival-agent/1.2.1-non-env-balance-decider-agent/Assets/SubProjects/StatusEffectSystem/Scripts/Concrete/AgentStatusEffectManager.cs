using UnityEngine;
using System.Collections.Generic;
using System;


public class AgentStatusEffectManager : MonoBehaviour {

    // Properties
    public List<BaseStatusEffectData> _effectsCanBeApplied;
    public List<BaseStatusEffectLogic> _activeEffects;

    private AgentStats _agentStats;

    // Unity Methods
    protected virtual void Start() {
        _agentStats = GetComponent<AgentStats>();
        _activeEffects = new List<BaseStatusEffectLogic>();
    }

    private void Update() {
        UpdateEffects(Time.deltaTime);
    }

    // Public Methods
    public void ApplyEffect(BaseStatusEffectData effectData) {
        if (effectData == null) return;


        uint effectIdToApply = effectData.ID;
        foreach (var activeEffect in _activeEffects) {
            if (activeEffect.ID == effectIdToApply) {
                activeEffect.OnEffectReset();
                return;
            }
        }

        BaseStatusEffectLogic effectLogic = effectData.CreateStatusEffectLogicInstance();
        effectLogic.Initialize(_agentStats, effectData);

        effectLogic.OnEffectReset();
        effectLogic.OnEffectApplied();
        _activeEffects.Add(effectLogic);
    }

    public void RemoveEffect(BaseStatusEffectLogic effectLogic) {
        if (effectLogic == null) return;

        effectLogic.OnEffectRemoved();
        _activeEffects.Remove(effectLogic);
    }

    // Private Methods
    private void UpdateEffects(float deltaTime) {
        if (_activeEffects == null) return;

        List<BaseStatusEffectLogic> effectsToRemove = new List<BaseStatusEffectLogic>();

        foreach (var effect in _activeEffects) { 
            effect.OnEffectUpdate(deltaTime);
            if (effect.IsExpired) {
                effectsToRemove.Add(effect);
            }
        }

        foreach (var expiredEffect in effectsToRemove) { 
            RemoveEffect(expiredEffect);
        }
    }


}
