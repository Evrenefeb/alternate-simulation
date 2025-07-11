using UnityEngine;

[CreateAssetMenu(fileName = "SO_RewardingData", menuName = "RewardingSystem/RewardingData")]
public class RewardingData : ScriptableObject
{
    [Header("Negative Rewards")]
    public float NR_Idle;
    public float NR_TakeDamage;
    public float NR_UnSuccessfulConsumeBase;
    public float NR_DeathPenalty;

    [Header("Positive Rewards")]
    public float PR_HealthRegenerated;
    [Range(0.5f, 2f)] public float PR_SuccessfulConsumeMultiplier;
    
}
