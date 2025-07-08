using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SimpleHealthGathererAgent : Agent {

    #region Properties
    private Rigidbody rb;
    public float forceAmount = 10f;
    public float rotationAmount = 200f;
    public float health = 100f;

    public Transform[] HealthOrbs;

    private bool _isMovingToTarget = false;
    private Transform _currentTargetOrb;
    private const float TARGET_REACHED_THRESHOLD = 0.5f;
    private const float STOP_MOVING_THRESHOLD = 0.1f;
    private const float STOP_VELOCITY_THRESHOLD = 0.05f;

    
    #endregion

    #region ActionMappings
    const int K_NOTHING = 0;
    const int K_0 = 1;
    const int K_1 = 2;
    const int K_2 = 3;
    #endregion

    #region UnityMethods
    private void Start() {
        if (rb == null) {
            rb = GetComponent<Rigidbody>();
        }
        if (HealthOrbs == null || HealthOrbs.Length == 0) {
            Debug.LogError("HealthOrbs array is not assigned or empty! Assign transforms in the Inspector.");
        }
    }
    #endregion

    #region MLAgentMethods
    public override void OnEpisodeBegin() {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        _isMovingToTarget = false;
        _currentTargetOrb = null;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(health);
        sensor.AddObservation(HealthOrbs[0].localPosition);
        sensor.AddObservation(HealthOrbs[1].localPosition);
        sensor.AddObservation(HealthOrbs[2].localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        AddReward(-1 / MaxStep);
        if (_isMovingToTarget && _currentTargetOrb != null) {
            ApplyForceToTarget(_currentTargetOrb);
            CheckIfTargetReached();
            return;
        }

        var action = actions.DiscreteActions[0];

        switch (action) {
            case K_0:
                if (HealthOrbs.Length > 0) SetNewTarget(HealthOrbs[0]);
                break;
            case K_1:
                if (HealthOrbs.Length > 1) SetNewTarget(HealthOrbs[1]);
                break;
            case K_2:
                if (HealthOrbs.Length > 2) SetNewTarget(HealthOrbs[2]);
                break;
            case K_NOTHING:
                StopMovement();
                break;
            default:
                StopMovement();
                break;
        }

        if (_isMovingToTarget && _currentTargetOrb != null) {
            ApplyForceToTarget(_currentTargetOrb);
            CheckIfTargetReached();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var discreteActionsOut = actionsOut.DiscreteActions;

        discreteActionsOut[0] = K_NOTHING;

        if (Input.GetKey(KeyCode.Alpha0)) {
            discreteActionsOut[0] = K_0;
        }
        else if (Input.GetKey(KeyCode.Alpha1)) {
            discreteActionsOut[0] = K_1;
        }
        else if (Input.GetKey(KeyCode.Alpha2)) {
            discreteActionsOut[0] = K_2;
        }
    }
    #endregion

    #region HelperMethods
    private void SetNewTarget(Transform targetOrb) {
        _currentTargetOrb = targetOrb;
        _isMovingToTarget = true;
    }

    private void ApplyForceToTarget(Transform targetOrb) {
        if (targetOrb == null) {
            StopMovement();
            return;
        }

        Vector3 distanceVec = targetOrb.position - transform.position;
        float distance = distanceVec.magnitude;

        if (distance > STOP_MOVING_THRESHOLD) {
            Vector3 moveDir = distanceVec.normalized;
            rb.AddForce(moveDir * forceAmount, ForceMode.Force);
        }
        else {
            StopMovement();
        }
    }

    private void CheckIfTargetReached() {
        if (_currentTargetOrb == null) return;

        float distance = Vector3.Distance(transform.position, _currentTargetOrb.position);

        if (distance < TARGET_REACHED_THRESHOLD) {
            Debug.Log($"Agent reached {_currentTargetOrb.name}!");
            _isMovingToTarget = false;
            _currentTargetOrb = null;
            StopMovement();
        }
    }

    private void StopMovement() {

        rb.linearVelocity *= 0.8f;
        if (rb.linearVelocity.magnitude < STOP_VELOCITY_THRESHOLD) {
            rb.linearVelocity = Vector3.zero;
        }
        rb.angularVelocity = Vector3.zero;
    }
    #endregion

}

