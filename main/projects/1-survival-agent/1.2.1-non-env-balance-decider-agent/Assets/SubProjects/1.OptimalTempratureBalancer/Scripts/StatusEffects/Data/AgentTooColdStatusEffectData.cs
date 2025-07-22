using UnityEngine;

[CreateAssetMenu(fileName = "AgentTooColdStatusEffectData", menuName = "OptimalTempratureBalancerEffect/AgentTooColdStatusEffectData")]
public class AgentTooColdStatusEffectData : BaseAgentStatusEffectData {

    [Header("Too Cold Related")]
    public float tempratureDecrementPerTick;
    public override BaseStatusEffectLogic CreateStatusEffectLogicInstance() {
        return new AgentTooColdStatusEffectLogic();
    }

}
