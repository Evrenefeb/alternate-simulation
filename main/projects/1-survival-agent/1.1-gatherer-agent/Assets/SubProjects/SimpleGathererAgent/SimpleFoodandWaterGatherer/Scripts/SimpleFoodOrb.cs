using UnityEngine;

public class SimpleFoodOrb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        other.gameObject.TryGetComponent<SimpleFoodandWaterGathererAgent>(out SimpleFoodandWaterGathererAgent agent);

        if(agent != null) {
            agent.CollectFood(gameObject);
        }
    }
}
