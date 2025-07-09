using UnityEngine;

public class WaterVolume : ConsumableEntity {
    public override void Consume(AdvancedFoodAndWaterGathererAgent agentStats) {
        agentStats.Water.ChangeWater(vitalAmount);
    }
}
