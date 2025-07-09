using UnityEngine;

public class AdvancedFoodAndWaterGathererAgent : MonoBehaviour
{
    private AgentStats agentStats;

    private void Awake() {
        agentStats = GetComponent<AgentStats>();
    }

    private void Update() {
        if (agentStats != null) {
            agentStats.Health.ApplyDamage(1f * Time.deltaTime);
        }
    }

}
