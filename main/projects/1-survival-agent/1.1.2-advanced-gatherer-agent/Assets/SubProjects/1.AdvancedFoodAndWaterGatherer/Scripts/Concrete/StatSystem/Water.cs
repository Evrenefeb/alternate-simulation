using UnityEngine;
using UnityEngine.Events;

public class Water
{

    // Properties
    public UnityAction<float> OnWaterChanged { get; set; }
    public UnityAction OnWaterEmpty { get; set; }

    private float maxWater;

    public float MaxWater { get => maxWater; set => maxWater = value; }

    public float CurrentWater { get; set; } = 0.0f;

    public bool IsFull => CurrentWater > MaxWater;
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
    public void SetWater(float food) {
        float previousWater = CurrentWater;

        CurrentWater = Mathf.Clamp(food, 0, MaxWater);

        float difference = food - previousWater;

        if (difference > 0.0f) {
            OnWaterChanged?.Invoke(difference);
        }
    }

    public void ChangeWater(float amount) {
        if (amount > 0) {
            if (IsFull) {
                return;
            }

            float previousWater = CurrentWater;

            CurrentWater += amount;

            CurrentWater = Mathf.Clamp(CurrentWater, 0, MaxWater);

            float changeAmount = CurrentWater - previousWater;

            if (changeAmount > 0.0f) {
                OnWaterChanged?.Invoke(changeAmount);
            }
        }
        else {
            if (IsEmpty) {
                return;
            }

            float previousWater = CurrentWater;

            CurrentWater = Mathf.Clamp(CurrentWater + amount, 0, MaxWater);

            float changeAmount = CurrentWater - previousWater;

            if (Mathf.Abs(changeAmount) > 0.0f) {
                OnWaterChanged?.Invoke(changeAmount);

                if (CurrentWater <= 0.0f) {
                    OnWaterEmpty?.Invoke();
                }
            }
        }
    }

}
