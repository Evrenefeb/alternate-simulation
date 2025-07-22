using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class OptimalTempratureBalancerAgent : Agent {

    // Components
    public AgentController AgentController { get; private set; }
    public AgentStats AgentStats { get; private set; }
    public AgentLocation AgentLocation { get; private set; }
    public AgentStatusEffectManager AgentStatusEffectManager { get; private set; }

    // Overriden Methods

    public override void Initialize() {
        AgentController = GetComponent<AgentController>();
        AgentStats = GetComponent<AgentStats>();
        AgentLocation = GetComponent<AgentLocation>();
        AgentStatusEffectManager = GetComponent<AgentStatusEffectManager>();
    }
    public override void OnEpisodeBegin() {
        
    }

    public override void CollectObservations(VectorSensor sensor) {
        
    }

    public override void OnActionReceived(ActionBuffers actions) {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        
    }




}

