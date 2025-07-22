using UnityEngine;

public class OptimalTempratureBalancerAgentStatusEffectManager : AgentStatusEffectManager {

    // Properties
    public float tooColdAfter = 0.8f;
    public float tooHotBetween = 0.15f;

    // Components
    public AgentLocation AgentLocation { get; private set; }

    protected override void Start() {
        base.Start();
        AgentLocation = GetComponent<AgentLocation>();
        AgentLocation.OnMeridianChanged += AgentLocation_OnMeridianChanged;
    }

    private void AgentLocation_OnMeridianChanged() {
        if(AgentLocation.Meridian > tooColdAfter || AgentLocation.Meridian < -tooColdAfter) {
            ApplyEffect(_effectsCanBeApplied[0]);
        } else if (AgentLocation.Meridian < tooHotBetween && AgentLocation.Meridian > -tooHotBetween) {
            ApplyEffect(_effectsCanBeApplied[1]);
        }
    }
}

