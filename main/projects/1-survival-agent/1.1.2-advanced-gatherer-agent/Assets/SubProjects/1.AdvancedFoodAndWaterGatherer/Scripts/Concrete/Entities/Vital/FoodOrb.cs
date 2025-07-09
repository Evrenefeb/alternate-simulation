using UnityEngine;

public class FoodOrb : ConsumableEntity {

    enum FoodState {
        Raw,
        Cooking,
        Cooked
    }
    [SerializeField] private FoodState foodState;
    public float cookedMultiplier;

    [SerializeField] private float cookTimer;
    [SerializeField] private float cookingTime = 30f;

    private void Start() {
        foodState = FoodState.Raw;   
    }

    

    public override void Consume(AdvancedFoodAndWaterGathererAgent agentStats) {
        if (foodState == FoodState.Cooked) {
            agentStats.Food.ChangeFood(vitalAmount * cookedMultiplier);
        }
        else {
            agentStats.Food.ChangeFood(vitalAmount);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.TryGetComponent<Campfire>(out Campfire campfire)) {
            if(foodState != FoodState.Cooked) {
                foodState = FoodState.Cooking;
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.TryGetComponent<Campfire>(out Campfire campfire)) {
            if(foodState != FoodState.Cooked) {
                foodState = FoodState.Raw;
            }
        }
    }

    private void GetCooked() {
        if(foodState != FoodState.Cooking) {
            return;
        }

        if (cookingTime > 0) { 
            cookingTime -= Time.deltaTime;
        }
        else {
            foodState = FoodState.Cooked;
        }
    }
    private void Update() { 
        GetCooked();
    }
}
