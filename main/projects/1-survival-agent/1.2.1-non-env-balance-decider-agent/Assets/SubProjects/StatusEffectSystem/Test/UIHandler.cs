using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text txt_Health;
    [SerializeField] private TMP_Text txt_Food;
    [SerializeField] private TMP_Text txt_MS;

    [SerializeField] private TMP_Text txt_AppliedEffects;

    [SerializeField] private GameObject agentRef;

    private AgentStatusEffectManager agentStatusEffectManager;
    private AgentStats agentStats;

    private void Start() {
        if(agentRef != null) {
            agentStats = agentRef.GetComponent<AgentStats>();
            agentStatusEffectManager = agentRef.GetComponent<AgentStatusEffectManager>();
        }
    }

    private void Update() {
        if(agentStats == null) return;

        txt_Health.text = agentStats.Health.ToString();
        txt_Food.text = agentStats.Food.ToString();
        txt_MS.text = agentStats.MovementSpeed.ToString();

        string effectText = null;
        foreach (var effect in agentStatusEffectManager._activeEffects) {
            effectText += $"ID:{effect.ID} - {effect.EffectName}\n";
        }
        txt_AppliedEffects.text = effectText;
    }
}
