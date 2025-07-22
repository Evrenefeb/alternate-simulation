using UnityEngine;

[CreateAssetMenu(fileName = "AgentWindyStatusEffectData", menuName = "OptimalTempratureBalancerEffect/AgentWindyStatusEffectData")]
public class AgentWindyStatusEffectData : BaseAgentStatusEffectData {

    [Header("Windy Related")]
    [Range(0f, 0.05f)]
    public float intensity;


    public override BaseStatusEffectLogic CreateStatusEffectLogicInstance() {
        return new AgentWindyStatusEffectLogic();
    }

}
