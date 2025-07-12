using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AdvancedFoodAndWaterGathererAgent : Agent {
    // Properties
    [Header("Initial Stats")]
    public float initialMaxHealth = 100f;
    public float initialMaxFood = 100f;
    public float initialMaxWater = 100f;
    public bool isAlive = true;

    [Header("Depletion Rates")]
    public float healthDepletionRate = 10f;
    public float foodDepletionRate = 5f;
    public float waterDepletionRate = 5f;

    [Header("Health Regen")]
    public float healthRegenRate = 2f;
    public float foodPercentThreshold = 0.5f;
    public float waterPercentThreshold = 0.5f;

    [Header("Movement")]
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;

    [Header("Consume Logic")]
    [SerializeField] private IConsumable currentConsumable;
    [SerializeField] private bool isBiting;
    [SerializeField] private bool canBite;

    [Header("Rewarding System")]
    public RewardingData rewardingData;


    [Header("Timers")]
    [SerializeField] private float depletionTimer;
    [SerializeField] private float biteCooldown;
    [SerializeField] private float repetativeConsumptionTimer;

    [Header("RepetativeConsumptions")]
    [SerializeField] private int repetativeConsumptionAmount; 

    // Events
    public event Action OnEpisodeEnd;

    // Components
    public Health Health { get; private set; }
    public Food Food { get; private set; }
    public Water Water { get; private set; }

    private Rigidbody rb;
    // Overridden Methods
    public override void Initialize() {
        // Initialize vital stats
        Health = new Health(initialMaxHealth);
        Food = new Food(initialMaxFood);
        Water = new Water(initialMaxWater);

        // Setup rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Initialize state
        isAlive = true;
        isBiting = false;
        canBite = true;
        depletionTimer = 0f;
        repetativeConsumptionTimer = 0f;
        repetativeConsumptionAmount = 0;
    }

    public override void OnEpisodeBegin() {
        // Reset all vital stats to maximum
        Health.SetHealth(initialMaxHealth);
        Food.SetFood(initialMaxFood);
        Water.SetWater(initialMaxWater);

        // Reset timers and state
        depletionTimer = 0f;
        isAlive = true;
        isBiting = false;
        canBite = true;
        currentConsumable = null;
        repetativeConsumptionTimer = 0f;
        repetativeConsumptionAmount = 0;

        // Reset physics
        if (rb != null) {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

    }

    public override void CollectObservations(VectorSensor sensor) {
        // Add vital stats as observations
        sensor.AddObservation(Health.GetPercentRatio());
        sensor.AddObservation(Food.GetPercentRatio());
        sensor.AddObservation(Water.GetPercentRatio());

        // Add action states
        sensor.AddObservation(isBiting ? 1f : 0f);
        sensor.AddObservation(isBiting ? 1f : 0f);
        sensor.AddObservation(currentConsumable != null ? 1f : 0f);

        // Add position for spatial awareness
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);
        sensor.AddObservation(transform.rotation.y);

        sensor.AddObservation(repetativeConsumptionAmount);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        if (!isAlive) return;

        // Process actions
        Movement(actions);
        AgentTryConsume(actions);


        // Check death condition
        CheckDeathCondition();
        CheckRepetativeConsumption();

        // Small penalty for time to encourage efficiency
        AddReward(rewardingData.NR_Idle);
    }

    private void CheckRepetativeConsumption() {
        repetativeConsumptionTimer += Time.fixedDeltaTime;

        if(repetativeConsumptionTimer > 30f) {
            repetativeConsumptionTimer = 0f;
            repetativeConsumptionAmount = 0;
        }
        else {
            if (repetativeConsumptionAmount > 4) {
                AddReward(rewardingData.NR_RepetativeConsumptionPenalty);
                repetativeConsumptionAmount--;
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");     // Forward/backward
        continuousActions[1] = Input.GetAxisRaw("Horizontal");   // Rotation

        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetKey(KeyCode.F) ? 1 : 0;    // Bite action
    }

    // Private Methods
    private void DepleteVitals() {
        if (Health == null || Food == null || Water == null) {
            Debug.LogError("One or more vital stats are null!");
            return;
        }



        if (depletionTimer >= 1f) {
            Food.ChangeFood(-foodDepletionRate);
            Water.ChangeWater(-waterDepletionRate);


            // Handle health logic
            if (Food.IsEmpty || Water.IsEmpty) {
                if (Health.IsAlive) {

                    Health.ApplyDamage(healthDepletionRate);
                    AddReward(rewardingData.NR_TakeDamage);
                }
            }
            else if (Food.GetPercentRatio() >= foodPercentThreshold && Water.GetPercentRatio() >= waterPercentThreshold) {
                Health.AddHealth(healthRegenRate);
                if (Health.GetPercentRatio() < 1f) {
                    AddReward(rewardingData.PR_HealthRegenerated);
                }
            }

            depletionTimer = 0f;
        }
    }

    private void Movement(ActionBuffers actions) {
        float moveZ = actions.ContinuousActions[0];
        float rotate = actions.ContinuousActions[1];

        // Apply movement
        Vector3 move = transform.TransformDirection(new Vector3(0f, 0f, moveZ));
        rb.linearVelocity = new Vector3(move.x * movementSpeed, rb.linearVelocity.y, move.z * movementSpeed);

        // Apply rotation
        transform.Rotate(0f, rotate * rotationSpeed * Time.deltaTime, 0f);
    }

    private void AgentTryConsume(ActionBuffers actions) {
        
        isBiting = false;

        if (actions.DiscreteActions[0] == 1 && canBite) {
            isBiting = true;

            if (currentConsumable != null) {
                currentConsumable.Consume(this);
                ConsumableEntity consumableEntity = currentConsumable as ConsumableEntity;
                AddReward((consumableEntity.vitalAmount * 0.01f) * rewardingData.PR_SuccessfulConsumeMultiplier);
                currentConsumable = null;
                repetativeConsumptionAmount++;
            }
            else {
                AddReward(rewardingData.NR_UnSuccessfulConsumeBase);
                currentConsumable = null;
            }

            canBite = false;
            biteCooldown = 3f;
        }
    }

    private void CheckDeathCondition() {
        if (!Health.IsAlive && isAlive) {
            isAlive = false;
            Debug.Log("Agent died!");
            SetReward(rewardingData.NR_DeathPenalty); // Set To Minimum
            OnEpisodeEnd?.Invoke();
            EndEpisode();
        }
    }

    // Collision Handling
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent<IConsumable>(out IConsumable consumable)) {
            currentConsumable = consumable;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.TryGetComponent<IConsumable>(out IConsumable consumable)) {
            if (currentConsumable == consumable) {
                currentConsumable = null;
            }
        }
    }

    private void FixedUpdate() {
        depletionTimer += Time.fixedDeltaTime;
        if (biteCooldown > 0) {
            biteCooldown -= Time.fixedDeltaTime;   
            canBite = false;
        }
        else {
            canBite = true;
        }
            DepleteVitals();
    }
}



