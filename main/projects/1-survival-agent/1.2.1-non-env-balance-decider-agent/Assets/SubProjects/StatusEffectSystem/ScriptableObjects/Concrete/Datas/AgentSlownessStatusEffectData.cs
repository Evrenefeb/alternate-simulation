using UnityEngine;

[CreateAssetMenu(fileName = "AgentSlownessStatusEffectData", menuName = "StatusEffectSystem/AgentSlownessStatusEffectData")]
public class AgentSlownessStatusEffectData : BaseAgentStatusEffectData {

    [Tooltip("Amount of Slowness to Apply")]
    public float SlownessFactor = 0.5f;

    public enum SlownessType {
        Subtract,
        Multiply
    }

    public SlownessType slownessType = SlownessType.Subtract;

    [Tooltip("Multiplier amount of applied effect")]
    [Range(1f, 3f)]
    public uint amplifier = 1;

    public override BaseStatusEffectLogic CreateStatusEffectLogicInstance() {
        return new AgentSlownessStatusEffectLogic();
    }
}
