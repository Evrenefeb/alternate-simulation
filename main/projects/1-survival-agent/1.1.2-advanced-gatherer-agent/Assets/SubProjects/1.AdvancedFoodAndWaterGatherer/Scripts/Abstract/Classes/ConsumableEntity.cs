using UnityEngine;

public abstract class ConsumableEntity : MonoBehaviour, IConsumable {

    public float vitalAmount;
    public abstract void Consume(AdvancedFoodAndWaterGathererAgent agentStats);
}
