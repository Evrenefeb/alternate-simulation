using UnityEngine;

[CreateAssetMenu(fileName = "AgentTooHotStatusEffectData", menuName = "OptimalTempratureBalancerEffect/AgentTooHotStatusEffectData")]
public class AgentTooHotStatusEffectData : BaseAgentStatusEffectData {

    [Header("Too Hot Related")]
    public float tempratureIncrementPerTick;
    public override BaseStatusEffectLogic CreateStatusEffectLogicInstance() {
        return new AgentTooHotStatusEffectLogic();
    }

}
