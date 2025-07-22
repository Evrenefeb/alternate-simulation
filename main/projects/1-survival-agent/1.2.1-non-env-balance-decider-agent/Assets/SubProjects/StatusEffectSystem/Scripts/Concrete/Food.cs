using UnityEngine;

public class Food : BaseStat {
    public float StarvationThresholdRatio = 0.2f;
    public Food(float initialMaxValue) : base(initialMaxValue) {
    }
}
