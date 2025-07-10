using UnityEngine;

public class FoodOrb : ConsumableEntity {

    enum FoodState {
        Raw,
        Cooking,
        Cooked
    }
    [SerializeField] private FoodState foodState;
    public float cookedMultiplier;

    [SerializeField] private float cookingTime = 30f;
    [SerializeField] private float initialCookingTime;

    private EnvironmentController environmentController;

    // Materials
    public Material M_RawFoodOrb;
    public Material M_CookedFoodOrb;
    public MeshRenderer SM_FoodOrb;


    private void Start() {
        initialCookingTime = cookingTime;
        foodState = FoodState.Raw;
        SM_FoodOrb.material = M_RawFoodOrb;
    }

    public void SetEnvironmentController(EnvironmentController controller)  {
        environmentController = controller;
    }

    public override void Consume(AdvancedFoodAndWaterGathererAgent agentStats) {
        if (foodState == FoodState.Cooked) {
            agentStats.Food.ChangeFood(vitalAmount * cookedMultiplier);
        }
        else {
            agentStats.Food.ChangeFood(vitalAmount);
        }
        ResetFoodOrb();
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
            SM_FoodOrb.material = M_CookedFoodOrb;
        }
    }
    private void Update() { 
        GetCooked();
    }

    private void ResetFoodOrb() {
        ResetFoodState();

        this.gameObject.SetActive(false);
        if(environmentController != null) {
            environmentController.RespawnFoodOrb(this.gameObject);
        }
    }

    private void ResetFoodState() {
        cookingTime = initialCookingTime;
        foodState = FoodState.Raw;
        SM_FoodOrb.material = M_RawFoodOrb;
    }
}
