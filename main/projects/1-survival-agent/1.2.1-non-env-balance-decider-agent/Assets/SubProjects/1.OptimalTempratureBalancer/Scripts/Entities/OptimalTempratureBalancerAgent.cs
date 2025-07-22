using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class OptimalTempratureBalancerAgent : Agent {

    // Properties
    public float sensivityThreshold = 0.15f;

    // Components
    public AgentController AgentController { get; private set; }
    public OptimalTempratureBalancerAgentStats AgentStats { get; private set; }
    public AgentLocation AgentLocation { get; private set; }
    public OptimalTempratureBalancerAgentStatusEffectManager AgentStatusEffectManager { get; private set; }

    // Overriden Methods

    public override void Initialize() {
        AgentController = GetComponent<AgentController>();
        AgentStats = GetComponent<OptimalTempratureBalancerAgentStats>();
        AgentLocation = GetComponent<AgentLocation>();
        AgentStatusEffectManager = GetComponent<OptimalTempratureBalancerAgentStatusEffectManager>();
    }
    public override void OnEpisodeBegin() {
        AgentStats.ResetStats();
    }

    public override void CollectObservations(VectorSensor sensor) { // 1 - 5
        foreach (var effect in AgentStatusEffectManager._activeEffects) { // 0 - 4 
            sensor.AddObservation(effect.ID);
        }
        sensor.AddObservation(AgentLocation.Meridian); // 1        
    }

    public override void OnActionReceived(ActionBuffers actions) {
        var continousActions = actions.ContinuousActions;
        float moveInput = continousActions[0];

        if(moveInput > sensivityThreshold) { AgentController.MoveNorth(); }
        if(moveInput < -sensivityThreshold) { AgentController.MoveSouth(); }

    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continousActions = actionsOut.ContinuousActions;
        float Y = Input.GetAxisRaw("Vertical");

        if(Y < -sensivityThreshold) { 
            continousActions[0] = -1f; 
            Debug.Log("Heuristic -1");
        }
        if(Y > sensivityThreshold) { 
            continousActions[0] = 1f;
            Debug.Log("Heuristic 1");

        }

    }




}

