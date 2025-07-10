using UnityEngine;
using UnityEngine.Events;

public class Water {
    // Properties
    public UnityAction<float> OnWaterChanged { get; set; }
    public UnityAction OnWaterEmpty { get; set; }

    private float maxWater;

    public float MaxWater { get => maxWater; set => maxWater = value; }

    public float CurrentWater { get; private set; } = 0.0f;

    public bool IsFull => CurrentWater >= MaxWater;
    public bool IsEmpty => CurrentWater <= 0.0f;

    // Constructors
    public Water(float maxWater) {
        this.maxWater = maxWater;
        SetWater(maxWater);
    }

    public Water() {
        maxWater = 100f;
        SetWater(maxWater);
    }

    // Public Methods
    public void SetWater(float water) {
        float previousWater = CurrentWater;
        CurrentWater = Mathf.Clamp(water, 0, MaxWater);
        float difference = CurrentWater - previousWater;

        if (Mathf.Abs(difference) > 0.0f) {
            OnWaterChanged?.Invoke(difference);
        }
    }

    public void ChangeWater(float amount) {
        float previousWater = CurrentWater;

        // Apply the change
        CurrentWater = Mathf.Clamp(CurrentWater + amount, 0, MaxWater);

        float changeAmount = CurrentWater - previousWater;

        // Only invoke events if there was actually a change
        if (Mathf.Abs(changeAmount) > 0.0f) {
            OnWaterChanged?.Invoke(changeAmount);

            // Check if we've reached empty after the change
            if (CurrentWater <= 0.0f) {
                OnWaterEmpty?.Invoke();
            }
        }

        // Debug logging
        Debug.Log($"Water Change: {amount} -> Current: {CurrentWater} (was {previousWater})");
    }

    public float GetPercentRatio() {
        return CurrentWater / MaxWater;
    }

    // Helper method for debugging
    public float GetCurrentWater() {
        return CurrentWater;
    }
}