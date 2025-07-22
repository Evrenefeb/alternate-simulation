using System;
using UnityEngine;

public class OptimalTempratureBalancerAgentStats : AgentStats {

    [Header("Initialzation")]
    public Conf_Init_OptimalTempratureBalancerStats initialStatData;

    // Stat Components
    public Temprature Temprature { get; private set; }
    public MovementStep MovementStep { get; private set; }


    // Unity Methods
    private void Awake() {
        Initialize();
    }

    // Private Methods
    private void Initialize() {
        Temprature = new Temprature(initialStatData.initialMaxTemperature, initialStatData.initialMinTemperature, initialStatData.optimumTemprature);
        MovementStep = new MovementStep(initialStatData.initialMaxMovementStep);
    }

    // Overriden Methods
    public override void ResetStats() {
        base.ResetStats();
        Temprature = null;
        MovementStep = null;

        Initialize();
    }
    

}

