using UnityEngine;
using UnityEngine.Events;

public class Health {

    // Properties
    public UnityAction<float> OnHealthChanged { get; set; }
    public UnityAction OnHealthEmpty { get; set; }

    private float maxHealth;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    public float CurrentHealth { get; private set; } = 0.0f;

    public bool IsAlive => CurrentHealth > 0.0f;

    // Constructors
    public Health(float maxHealth) {
        this.maxHealth = maxHealth;
        SetHealth(maxHealth);
    }

    public Health() {
        maxHealth = 100f;
        SetHealth(maxHealth);
    }

    // Public Methods
    public void SetHealth(float health) {
        float previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Clamp(health, 0, MaxHealth);
        float difference = CurrentHealth - previousHealth;

        if (Mathf.Abs(difference) > 0.0f) {
            OnHealthChanged?.Invoke(difference);
        }
    }

    public void AddHealth(float amount) {
        if (!IsAlive) {
            return;
        }

        float previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        float changeAmount = CurrentHealth - previousHealth;

        if (changeAmount > 0.0f) {
            OnHealthChanged?.Invoke(changeAmount);
        }

        Debug.Log($"Health Added: {amount} -> Current: {CurrentHealth} (was {previousHealth})");
    }

    /// <summary>
    /// Applies damage to the health (Positive float)
    /// </summary>
    /// <param name="damage"></param>
    public void ApplyDamage(float damage) {
        if (!IsAlive) {
            return;
        }

        float previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        float changeAmount = CurrentHealth - previousHealth;

        if (Mathf.Abs(changeAmount) > 0.0f) {
            OnHealthChanged?.Invoke(changeAmount);

            if (CurrentHealth <= 0.0f) {
                OnHealthEmpty?.Invoke();
            }
        }

        Debug.Log($"Health Damage: {damage} -> Current: {CurrentHealth} (was {previousHealth})");
    }

    public float GetPercentRatio() {
        return CurrentHealth / MaxHealth;
    }

    // Helper method for debugging
    public float GetCurrentHealth() {
        return CurrentHealth;
    }
}