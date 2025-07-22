using System;
using UnityEngine;

public class AgentController : MonoBehaviour {

    // Components
    private AgentLocation AgentLocation;
    private OptimalTempratureBalancerAgentStats AgentStats;

    // Unity Methods
    private void Start() {
        AgentLocation = GetComponentInParent<AgentLocation>();
        AgentStats = GetComponentInParent<OptimalTempratureBalancerAgentStats>();
    }

    // Controller Methods
    public bool MoveNorth() {
        if (AgentLocation.Meridian != 1f) return false;
        AgentLocation.Meridian += AgentStats.MovementStep.CurrentValue;
        return true;
    }

    public bool MoveSouth() {
        if (AgentLocation.Meridian != -1f) return false;
        AgentLocation.Meridian -= AgentStats.MovementStep.CurrentValue;
        return true;
    }

}
