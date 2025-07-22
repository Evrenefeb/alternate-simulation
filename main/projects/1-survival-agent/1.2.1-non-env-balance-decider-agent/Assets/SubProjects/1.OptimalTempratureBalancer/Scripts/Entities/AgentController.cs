using System;
using UnityEngine;

public class AgentController : MonoBehaviour {

    // Properties
    public float movementSpeed;

    // Components
    private AgentLocation AgentLocation;

    // Unity Methods
    private void Start() {
        AgentLocation = GetComponentInParent<AgentLocation>();
    }

    // Controller Methods
    public bool MoveNorth() {
        if (AgentLocation.Meridian != 1f) return false;
        AgentLocation.Meridian += movementSpeed;
        return true;
    }

    public bool MoveSouth() {
        if (AgentLocation.Meridian != -1f) return false;
        AgentLocation.Meridian -= movementSpeed;
        return true;
    }

}
