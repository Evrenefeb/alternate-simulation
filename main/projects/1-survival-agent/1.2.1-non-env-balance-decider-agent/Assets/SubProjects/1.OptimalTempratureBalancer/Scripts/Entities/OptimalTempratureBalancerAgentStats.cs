using UnityEngine;

public class OptimalTempratureBalancerAgentStats : MonoBehaviour {

    [Header("Initialzation")]
    public Conf_Init_OptimalTempratureBalancerStats initialStatData;

    // Stat Components
    public Temprature Temprature { get; private set; }
    public MovementStep MovementStep { get; private set; }

    private void Awake() {
        Temprature = new Temprature(initialStatData.initialMaxTemperature, initialStatData.initialMinTemperature, initialStatData.optimumTemprature);
        MovementStep = new MovementStep(initialStatData.initialMaxMovementStep);
    }

}

