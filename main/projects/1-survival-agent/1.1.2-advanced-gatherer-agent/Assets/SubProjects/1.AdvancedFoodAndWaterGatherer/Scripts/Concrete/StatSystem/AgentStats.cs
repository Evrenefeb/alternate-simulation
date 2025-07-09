using UnityEngine;

public class AgentStats : MonoBehaviour
{
    // Properties
    [Header("Initial Stats")]
    public float initialmaxHealth;
    public float initialmaxFood;
    public float initialmaxWater;

    // Components

    public Health Health { get; private set; }
    public Food Food { get; private set; }
    public Water Water { get; private set; }


    private void Awake() {
        Health = new Health(initialmaxHealth);
        Food = new Food(initialmaxFood);
        Water = new Water(initialmaxWater);
    }
}
