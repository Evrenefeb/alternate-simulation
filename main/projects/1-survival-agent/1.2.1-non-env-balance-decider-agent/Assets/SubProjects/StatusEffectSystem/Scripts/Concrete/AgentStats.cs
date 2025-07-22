using UnityEngine;

public class AgentStats : MonoBehaviour
{
    [Header("Initial")]

    public float initialMaxHealth;
    public float initialMaxFood;
    public float initialMaxMS;

    public float foodDepletionRate;

    // Stat Components
    public Health Health { get; private set; }
    public Food Food { get; private set; }
    public MovementSpeed MovementSpeed { get; private set; }

    // Control Components
    private AgentStatusEffectManager _agentStatusEffectManager;

    private void Awake() {
        Health = new Health(initialMaxHealth);
        Food = new Food(initialMaxFood);
        MovementSpeed = new MovementSpeed(initialMaxMS);

        _agentStatusEffectManager = GetComponent<AgentStatusEffectManager>();
    }

    private void Update() {
        // Test
        Food.CurrentValue -= Time.deltaTime * foodDepletionRate;
        if(Food.GetRatio() < Food.StarvationThresholdRatio) {
            _agentStatusEffectManager.ApplyEffect(_agentStatusEffectManager._effectsCanBeApplied[1]);
        }
        
    }

    // Public Methods
    public virtual void ResetStats() {
        Health = null;
        Food = null;
        MovementSpeed = null;

        Health = new Health(initialMaxHealth);
        Food = new Food(initialMaxFood);
        MovementSpeed = new MovementSpeed(initialMaxMS);
    }
}
