using UnityEngine;

public class SimpleWaterVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        SimpleFoodandWaterGathererAgent agent = other.GetComponent<SimpleFoodandWaterGathererAgent>();
        if (agent != null) {
            
            Debug.Log("Agent entered water volume");
        }
    }

    private void OnTriggerExit(Collider other) {
        SimpleFoodandWaterGathererAgent agent = other.GetComponent<SimpleFoodandWaterGathererAgent>();
        if (agent != null) {
            Debug.Log("Agent left water volume");
        }
    }
}
