using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;


public class SimpleFoodandWaterGathererAgent : Agent {
    [Header("Agent Stats")]
    public int maxHealth = 100;
    public int maxFood = 100;
    public int maxWater = 100;
    public int currentHealth = 50;
    public int currentFood = 50;
    public int currentWater = 50;

    [Header("Depletion Rates")]
    public float foodDepletionRate = 1f;
    public float waterDepletionRate = 1f;
    public float healthDepletionRate = 5f;

    [Header("Restoration Amounts")]
    public int foodOrbRestoreAmount = 20;
    public int waterVolumeRestoreAmount = 15;

    [Header("Environment Settings")]
    public float episodeTimeLimit = 60f;
    public Vector3 environmentSize;
    public int numberOfFoodOrbs = 3;

    public enum SubExperimentSettings {
        FixedAgent_RandomFood_FixedWater,
        RandomAgent_RandomFood_FixedWater,
        RandomAll
    }
    public SubExperimentSettings subExperimentSettings = SubExperimentSettings.FixedAgent_RandomFood_FixedWater;

    [Header("Related GameObjects")]
    public GameObject foodOrbPrefab;
    public GameObject waterVolumePrefab;
    public Transform waterVolumeTransform;

    private List<GameObject> foodOrbs = new List<GameObject>();
    protected float episodeTimer = 0f;
    protected float depletionTimer = 0f;
    private Vector3 initialAgentPosition;
    private Vector3 initialWaterPosition;
    private Rigidbody rb;

    [Header("Movement")]
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;

    [Header("Rewards")]
    public float R_OnDeath = -10f;
    public float R_Success = 5f;
    public float R_GatheredWater = 0.1f;
    public float R_GatheredFood = 0.5f;

    [Header("Display")]
    public TMP_Text txt_AgentCumulativeReward;
    public TMP_Text txt_EpisodeTime;


    // Overriden Methods

    public override void Initialize() {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        initialAgentPosition = transform.position;
        if (waterVolumeTransform != null) {
            initialWaterPosition = waterVolumeTransform.position;
        }

        
        if (waterVolumeTransform == null && waterVolumePrefab != null) {
            GameObject waterVolume = Instantiate(waterVolumePrefab);
            waterVolumeTransform = waterVolume.transform;
            initialWaterPosition = waterVolumeTransform.position;
        }
    }

    public override void OnEpisodeBegin() {
        currentHealth = Mathf.RoundToInt(maxHealth / 2);
        currentFood = Mathf.RoundToInt(maxFood / 2);
        currentWater = Mathf.RoundToInt(maxWater / 2);
        episodeTimer = 0f;
        depletionTimer = 0f;

        foreach (GameObject orb in foodOrbs) {
            if (orb != null)
                DestroyImmediate(orb);
        }
        foodOrbs.Clear();

        PositionAgent();
        PositionWaterVolume();
        SpawnFoodOrbs();

        if (rb != null) {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(currentHealth);
        sensor.AddObservation(currentFood);
        sensor.AddObservation(currentWater);

        for (int i = 0; i < 3; i++) {
            if (i < foodOrbs.Count && foodOrbs[i] != null) {
                Vector3 localPos = transform.InverseTransformPoint(foodOrbs[i].transform.position);
                sensor.AddObservation(localPos.x);
                sensor.AddObservation(localPos.y);
                sensor.AddObservation(localPos.z);
            }
            else {
                sensor.AddObservation(0f);
                sensor.AddObservation(0f);
                sensor.AddObservation(0f);
            }
        }

        if (waterVolumeTransform != null) {
            Vector3 localWaterPos = transform.InverseTransformPoint(waterVolumeTransform.position);
            sensor.AddObservation(localWaterPos.x);
            sensor.AddObservation(localWaterPos.y);
            sensor.AddObservation(localWaterPos.z);
        }
        else {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float rotate = actions.ContinuousActions[1];

        Vector3 move = transform.TransformDirection(new Vector3(moveX, 0f, 0f));
        rb.linearVelocity = move * movementSpeed;

        transform.Rotate(0, rotate * rotationSpeed * Time.deltaTime, 0);

        episodeTimer += Time.deltaTime;
        depletionTimer += Time.deltaTime;

        txt_AgentCumulativeReward.SetText(GetCumulativeReward().ToString());
        txt_EpisodeTime.SetText(episodeTimer.ToString());

        if (depletionTimer >= 1f) {
            currentFood = Mathf.Clamp(currentFood - Mathf.RoundToInt(foodDepletionRate), 0, maxFood);
            currentWater = Mathf.Clamp(currentWater - Mathf.RoundToInt(waterDepletionRate), 0, maxWater);
            depletionTimer = 0f;

            if (currentFood <= 0 || currentWater <= 0) {
                currentHealth = Mathf.Clamp(currentHealth - Mathf.RoundToInt(healthDepletionRate), 0, maxHealth);
            }
        }

        CheckWaterVolume();

        float R_Calculated = CalculateReward();
        AddReward(R_Calculated);

        if (currentHealth <= 0) {
            AddReward(R_OnDeath);
            EndEpisode();
        }
        else if (episodeTimer >= episodeTimeLimit) {
            AddReward(R_Success);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");
        continuousActions[1] = Input.GetKey(KeyCode.Q) ? -1f : Input.GetKey(KeyCode.E) ? 1f : 0f;
    }

    // Private Methods

    private void PositionAgent() {
        switch (subExperimentSettings) {
            case SubExperimentSettings.FixedAgent_RandomFood_FixedWater:
                transform.position = initialAgentPosition;
                break;
            case SubExperimentSettings.RandomAgent_RandomFood_FixedWater:
            case SubExperimentSettings.RandomAll:
                transform.position = GetRandomPosition();
                break;
        }

        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    private Vector3 GetRandomPosition() {
        Vector3 environmentCenter = transform.position;
        float x = Random.Range(-environmentSize.x / 2f, environmentSize.x / 2f);
        float z = Random.Range(-environmentSize.z / 2f, environmentSize.z / 2f);
        return new Vector3(environmentCenter.x + x, environmentCenter.y, environmentCenter.z + z);
    }

    private Vector3 GetRandomPositionLocal() {
        float x = Random.Range(-environmentSize.x / 2f, environmentSize.x / 2f);
        float z = Random.Range(-environmentSize.z / 2f, environmentSize.z / 2f);
        return new Vector3(x, transform.position.y, z);
    }

    private void PositionWaterVolume() {
        if (waterVolumeTransform == null) return;

        switch (subExperimentSettings) {
            case SubExperimentSettings.FixedAgent_RandomFood_FixedWater:
            case SubExperimentSettings.RandomAgent_RandomFood_FixedWater:
                waterVolumeTransform.position = initialWaterPosition;
                break;
            case SubExperimentSettings.RandomAll:
                waterVolumeTransform.position = GetRandomPositionLocal();
                break;
        }
    }

    private void SpawnFoodOrbs() {
        for (int i = 0; i < numberOfFoodOrbs; i++) {
            if (foodOrbPrefab != null) {
                Vector3 spawnPos = GetRandomPosition();
                GameObject orb = Instantiate(foodOrbPrefab, spawnPos, Quaternion.identity);

                if (orb.GetComponent<SimpleFoodOrb>() == null) {
                    orb.AddComponent<SimpleFoodOrb>();
                }

                foodOrbs.Add(orb);
                orb.transform.SetParent(transform.parent);

            }
        }
    }

    private bool CheckWaterVolume() {

        if (waterVolumeTransform != null) {
            float distance = Vector3.Distance(transform.position, waterVolumeTransform.position);
            if (distance < 2f) {

                if (currentWater < maxWater) {
                    currentWater = Mathf.Clamp(currentWater + Mathf.RoundToInt(waterVolumeRestoreAmount), 0, maxWater);
                    AddReward(R_GatheredWater);
                }


            }

            return true;
        }

        return false;
    }

    

    //  Public Methods

    public void CollectFood(GameObject foodOrb) {
        if (foodOrbs.Contains(foodOrb)) {
            currentFood = Mathf.Clamp(currentFood + foodOrbRestoreAmount, 0, maxFood);
            AddReward(R_GatheredFood);

            foodOrbs.Remove(foodOrb);

            foodOrb.transform.position = GetRandomPosition();
            foodOrbs.Add(foodOrb);
        }
    }

    // Protected Methods

    protected virtual float CalculateReward() {
        float healthReward = (float)currentHealth / maxHealth * 0.01f;
        float foodReward = (float)currentFood / maxFood * 0.01f;
        float waterReward = (float)currentWater / maxWater * 0.01f;

        return (healthReward + foodReward + waterReward) * Time.deltaTime;
    }


}
