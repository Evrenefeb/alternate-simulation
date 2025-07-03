using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GathererAgent : Agent 
{
    public int health;
    public float forceAmount;
    public float rotationAmount;

    public GameObject[] HealthOrbs = new GameObject[3];


    private Rigidbody rb;

    [Header("Cumulative Reward")]
    [SerializeField] private float CumulativeReward;

    #region ActionMappings

    const int K_NOTHING = 0;
    const int K_W = 1;
    const int K_Q = 2;
    const int K_E = 3;

    #endregion

    protected override void Awake() {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }


    public override void Initialize() {
        health = 50;
        rb = GetComponent<Rigidbody>();
        transform.localPosition = Vector3.zero;
        SetReward(0f);
    }

    public override void OnEpisodeBegin() {
        health = 50;
        transform.localPosition = Vector3.zero;
        SetReward(0f);
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(health); // 1
        sensor.AddObservation(HealthOrbs[0].transform.localPosition.x);
        sensor.AddObservation(HealthOrbs[0].transform.localPosition.y);
        sensor.AddObservation(HealthOrbs[0].transform.localPosition.z);
        sensor.AddObservation(HealthOrbs[1].transform.localPosition.x);
        sensor.AddObservation(HealthOrbs[1].transform.localPosition.y);
        sensor.AddObservation(HealthOrbs[1].transform.localPosition.z);
        sensor.AddObservation(HealthOrbs[2].transform.localPosition.x);
        sensor.AddObservation(HealthOrbs[2].transform.localPosition.y);
        sensor.AddObservation(HealthOrbs[2].transform.localPosition.z);
        
    }

    private void Movement(ActionSegment<int> actions) {

        var action = actions[0];

        switch (action) {
            case K_W:
                rb.AddRelativeForce(Vector3.forward * forceAmount);
                break;
            case K_Q:
                transform.Rotate(Vector3.up * rotationAmount * Time.deltaTime);
                break;
            case K_E:
                transform.Rotate(Vector3.down * rotationAmount * Time.deltaTime);
                break;
            case K_NOTHING:
                break;
            default:
                break;
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        Movement(actions.DiscreteActions);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {

        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = K_NOTHING;

        if (Input.GetKey(KeyCode.W)) {
            discreteActionsOut[0] = K_W;
        }
        else if (Input.GetKey(KeyCode.Q)) {
            discreteActionsOut[0] = K_Q;
        }
        else if (Input.GetKey(KeyCode.E)) { 
            discreteActionsOut[0] = K_E;
        }
    }


    private void Update() {
        CumulativeReward = GetCumulativeReward();
        if(health < 0) {
            SetReward(-1f);
            EndEpisode();
        }
        if(CumulativeReward > 1) {
            SetReward(1f);
            EndEpisode();
        }
    }








}
