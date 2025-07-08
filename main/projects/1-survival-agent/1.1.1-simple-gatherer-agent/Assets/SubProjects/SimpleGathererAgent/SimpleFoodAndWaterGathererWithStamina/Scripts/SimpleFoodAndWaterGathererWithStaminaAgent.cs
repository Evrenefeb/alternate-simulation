using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SimpleFoodAndWaterGathererWithStaminaAgent : SimpleFoodandWaterGathererAgent
{
    [Header("Stamina Stats")]
    public float maxStamina = 100f;
    public float currentStamina = 50f;
    public float staminaDepletionRate = 10f;
    public float staminaRegenRate = 5f;
    public float sprintMultiplier = 2f;
    public float initialMovementSpeed;
    public float maxMovementSpeed = 5f;

    [Header("Stamina Rewards")]
    public float R_Tired = -0.1f;

    // Overriden Methods

    public override void OnEpisodeBegin() {        
        base.OnEpisodeBegin();
        currentStamina = Mathf.RoundToInt(maxStamina / 2);
        initialMovementSpeed = movementSpeed;
    }

    public override void CollectObservations(VectorSensor sensor) {
        base.CollectObservations(sensor);
        sensor.AddObservation(currentStamina);
    }

    public override void OnActionReceived(ActionBuffers actions) {

        int sprint = actions.DiscreteActions[0];
        switch (sprint) {
            case 0:
                movementSpeed = initialMovementSpeed;
                currentStamina = Mathf.Clamp(currentStamina + staminaRegenRate * Time.deltaTime, -10f, maxStamina);
                break;
            case 1:
                if(currentStamina > 0f) {
                    movementSpeed = Mathf.Min(initialMovementSpeed * sprintMultiplier, maxMovementSpeed);

                    currentStamina = Mathf.Clamp(currentStamina - staminaDepletionRate * Time.deltaTime, -10f, maxStamina);
                }
                else {
                    movementSpeed = initialMovementSpeed;
                    AddReward(R_Tired);
                }
                break;
        }


        if (currentStamina <= 0) {
            AddReward(R_Tired * Time.deltaTime);
        }

        base.OnActionReceived(actions);


        
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        base.Heuristic(actionsOut);

        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetKey(KeyCode.LeftShift) ? 1 : 0;
    }

    protected override float CalculateReward() {
        float staminaReward = (currentStamina < 0f) ? (currentStamina / maxStamina) * 0.05f : (currentStamina / maxStamina) * 0.01f;
        return base.CalculateReward() + staminaReward;
    }

}
