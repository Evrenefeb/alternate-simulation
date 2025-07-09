using TMPro;
using UnityEngine;

public class AgentStatDisplayer : MonoBehaviour {
    public Transform lookAtLoc;
    private Transform localTransform;

    private AgentStats agentStats;

    [SerializeField] private TMP_Text txt_Health;
    [SerializeField] private TMP_Text txt_Food;
    [SerializeField] private TMP_Text txt_Water;

    private void Start() {
        localTransform = GetComponent<Transform>();
        agentStats = GetComponentInParent<AgentStats>();
    }

    private void Update() {
        localTransform.LookAt(2 * localTransform.position - lookAtLoc.position);
        UpdateStatTexts();
    }

    private void UpdateStatTexts() {
        txt_Health.text = Mathf.RoundToInt(agentStats.Health.CurrentHealth).ToString() + " / " + Mathf.RoundToInt(agentStats.Health.MaxHealth).ToString();
        txt_Food.text = Mathf.RoundToInt(agentStats.Food.CurrentFood).ToString() + " / " + Mathf.RoundToInt(agentStats.Food.MaxFood).ToString();
        txt_Water.text = Mathf.RoundToInt(agentStats.Water.CurrentWater).ToString() + " / " + Mathf.RoundToInt(agentStats.Water.MaxWater).ToString();
    }
    
}
