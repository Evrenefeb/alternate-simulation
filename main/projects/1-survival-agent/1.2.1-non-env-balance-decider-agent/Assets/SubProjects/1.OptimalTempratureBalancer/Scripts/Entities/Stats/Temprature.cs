using UnityEngine;

public class Temprature : BaseStat {
    public float OptimumTemprature { get; private set; }
    public Temprature(float initialMaxValue, float initialMinValue, float optimumTemprature) : base(initialMaxValue, initialMinValue) {
        OptimumTemprature = optimumTemprature;
    }
}

