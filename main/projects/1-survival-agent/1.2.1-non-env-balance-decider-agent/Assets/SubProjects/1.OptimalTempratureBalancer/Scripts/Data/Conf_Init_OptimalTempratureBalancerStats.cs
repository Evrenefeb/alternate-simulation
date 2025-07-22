using UnityEngine;

[CreateAssetMenu(fileName = "Initial Stats", menuName = "Config/Conf_Init_OptimalTempratureBalancerStats")]
public class Conf_Init_OptimalTempratureBalancerStats : ScriptableObject
{
    public float initialMaxTemperature;
    public float initialMinTemperature;
    public float optimumTemprature;
    [Range(0f, 0.1f)]
    public float initialMaxMovementStep;
}
