using UnityEngine;

[CreateAssetMenu(fileName = "AgentStarvingStatusEffectData", menuName = "StatusEffectSystem/AgentStarvingStatusEffectData")]
public class AgentStarvingStatusEffectData : BaseAgentStatusEffectData {

    [Tooltip("Food Stat to loose every tick")]
    public float starvationPerTick;

    public override BaseStatusEffectLogic CreateStatusEffectLogicInstance() {
        return new AgentStarvingStatusEffectLogic();
    }
}
