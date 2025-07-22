using UnityEngine;

public abstract class BaseAgentStatusEffectData : BaseStatusEffectData
{
    [Tooltip("Obtain Cumulative Reward on Tick")]
    public float RewardPerTick = 0f;
}
