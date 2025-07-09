using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AdvancedFoodAndWaterGathererAgent : Agent
{
    // Properties
    [Header("Initial Stats")]
    public float initialMaxHealth;
    public float initialMaxFood;
    public float initialMaxWater;
    public bool isAlive;

    [Header("Depletion Rates")]
    public float healthDepletionRate;
    public float foodDepletionRate;
    public float waterDepletionRate;

    [Header("Health Regen")]
    public float healthRegenRate;
    public float foodPercentThreshold;
    public float waterPercentThreshold;

    [Header("Movement")]
    public float movementSpeed;
    public float rotationSpeed;

    // Components

    public Health Health { get; private set; }
    public Food Food { get; private set; }
    public Water Water { get; private set; }  
    
    private Rigidbody rb;

    // Timers

    protected float depletionTimer;

    // Events

    public event Action OnEpisodeEnd;

    // Overriden Methods

    public override void Initialize() {
        Health = new Health(initialMaxHealth);
        Food = new Food(initialMaxFood);
        Water = new Water(initialMaxWater);   
        
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        isAlive = true;
    }

    public override void OnEpisodeBegin() {
        Health.SetHealth(initialMaxHealth);
        Food.SetFood(initialMaxFood);
        Water.SetWater(initialMaxWater);

        depletionTimer = 0f;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(Health.GetPercentRatio());
        sensor.AddObservation(Food.GetPercentRatio());
        sensor.AddObservation(Water.GetPercentRatio());
    }

    public override void OnActionReceived(ActionBuffers actions) {
        Movement(actions);
        DepleteVitals();        
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");
        continuousActions[1] = Input.GetKey(KeyCode.Q) ? -1f : Input.GetKey(KeyCode.E) ? 1f : 0f;
    }

    // Private Methods
    
    private void DepleteVitals() {
        if(Health == null || Food == null || Water == null) {
            return;
        }

        depletionTimer += Time.deltaTime;

        if(depletionTimer >= 1f) {
            Food.ChangeFood(foodDepletionRate);
            Water.ChangeWater(waterDepletionRate);

            depletionTimer = 0f;

            if(Food.IsEmpty || Water.IsEmpty) {

                if (Health.IsAlive) {
                    Health.ApplyDamage(healthDepletionRate);
                }
                else {
                    // Agent Died
                }


            }else if (Food.GetPercentRatio() >= foodPercentThreshold && Water.GetPercentRatio() >= waterPercentThreshold) {
                Health.AddHealth(healthRegenRate);
            }
        }               
    }

    private void Movement(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float rotate = actions.ContinuousActions[1];

        Vector3 move = transform.TransformDirection(new Vector3(moveX, 0f, 0f));
        rb.linearVelocity = move * movementSpeed;

        transform.Rotate(0f, rotate * rotationSpeed * Time.deltaTime, 0f);
    }
    
}
