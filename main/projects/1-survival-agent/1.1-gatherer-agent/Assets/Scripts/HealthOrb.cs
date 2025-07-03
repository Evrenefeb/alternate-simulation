using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    public int amount;

    private void Awake() {
        Vector3 newLoc = new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10));
        transform.localPosition = newLoc;
    }

    private void OnTriggerEnter(Collider other) {
        other.TryGetComponent<GathererAgent>(out GathererAgent agent);

        if(agent != null) {
            agent.health += amount;
            agent.AddReward(0.25f);
            Vector3 newLoc = new Vector3 (Random.Range(-20, 20), 0.5f , Random.Range(-20, 20));
            transform.localPosition = newLoc;
        }
    }
}
