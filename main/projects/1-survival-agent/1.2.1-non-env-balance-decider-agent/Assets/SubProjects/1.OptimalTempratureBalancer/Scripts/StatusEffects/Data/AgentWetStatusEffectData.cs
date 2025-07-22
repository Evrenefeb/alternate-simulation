using UnityEngine;

[CreateAssetMenu(fileName = "AgentWetStatusEffectData", menuName = "OptimalTempratureBalancerEffect/AgentWetStatusEffectData")]
public class AgentWetStatusEffectData : BaseAgentStatusEffectData {

    [Header("Wet Related")]
    
    public float wetColdThreshold = 5; // below loose reward - above null
    public float wetHotThreshold = 30; // above gain reward - below null

    public float coolingRate;

    public float freezingReward; // negative
    public float coolingReward; // positive


    public override BaseStatusEffectLogic CreateStatusEffectLogicInstance() {
        return new AgentWetStatusEffectLogic();
    }

}
