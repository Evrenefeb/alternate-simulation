using UnityEngine;
using UnityEngine.Events;

public class Food
{

    // Properties
    public UnityAction<float> OnFoodChanged { get; set; }
    public UnityAction OnFoodEmpty { get; set; }

    private float maxFood;

    public float MaxFood { get => maxFood; set => maxFood = value; }

    public float CurrentFood { get; private set; } = 0.0f;

    public bool IsFull => CurrentFood > MaxFood;
    public bool IsEmpty => CurrentFood <= 0.0f;


    // Constructors
    public Food(float maxFood) {
        this.maxFood = maxFood;
        SetFood(maxFood);
    }

    public Food() {
        maxFood = 100f;
        SetFood(maxFood);
    }


    // Public Methods
    public void SetFood(float food) {
        float previousFood = CurrentFood;

        CurrentFood = Mathf.Clamp(food, 0, MaxFood);

        float difference = food - previousFood;

        if (difference > 0.0f) {
            OnFoodChanged?.Invoke(difference);
        }
    }

    public void ChangeFood(float amount) {
        if (amount > 0) {
            if (IsFull) {
                return;
            }

            float previousFood = CurrentFood;

            CurrentFood += amount;

            CurrentFood = Mathf.Clamp(CurrentFood, 0, MaxFood);

            float changeAmount = CurrentFood - previousFood;

            if (changeAmount > 0.0f) {
                OnFoodChanged?.Invoke(changeAmount);
            }
        }
        else {
            if (IsEmpty) {
                return;
            }

            float previousFood = CurrentFood;

            CurrentFood = Mathf.Clamp(CurrentFood + amount, 0, MaxFood);

            float changeAmount = CurrentFood - previousFood;

            if (Mathf.Abs(changeAmount) > 0.0f) {
                OnFoodChanged?.Invoke(changeAmount);

                if (CurrentFood <= 0.0f) {
                    OnFoodEmpty?.Invoke();
                }
            }
        }
    }

    public float GetPercentRatio() {
        return CurrentFood / MaxFood;
    }

}
