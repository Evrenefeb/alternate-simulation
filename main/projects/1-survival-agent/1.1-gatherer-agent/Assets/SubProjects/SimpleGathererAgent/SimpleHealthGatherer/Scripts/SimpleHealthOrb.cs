using UnityEngine;

public class SimpleHealthOrb : MonoBehaviour
{
    public float amount = 20f;

    private void Awake() {
        Vector3 newLoc = new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10));
        transform.localPosition = newLoc;
    }

    private void OnTriggerEnter(Collider other) {
        other.TryGetComponent<SimpleHealthGathererAgent>(out SimpleHealthGathererAgent agent);

        if(agent != null) {
            agent.health += amount;
            agent.AddReward(0.25f);
            Vector3 newLoc = new Vector3 (Random.Range(-20, 20), 0.5f , Random.Range(-20, 20));
            transform.localPosition = newLoc;
        }
    }
}
