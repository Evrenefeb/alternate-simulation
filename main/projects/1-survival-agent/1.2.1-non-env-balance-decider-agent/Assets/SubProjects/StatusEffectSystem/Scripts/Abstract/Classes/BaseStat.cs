using System;
using UnityEngine;

public abstract class BaseStat : IStat {

    // Properties
    private float _currentValue;
    private float _maxValue;
    private float _minValue;

    public float CurrentValue {
        get {
            return _currentValue;
        }
        set {
            if (value < 0) value = 0;
            if (value >= MaxValue) value = MaxValue;
            _currentValue = value;
        }
    }
    public float MaxValue {
        get {
            return _maxValue;
        }
        set {
            if (value < MinValue) value = MinValue;
            if (value < _maxValue && value > CurrentValue) {
                CurrentValue = value;
            }
            _maxValue = value;
        }
    }

    public float MinValue {
        get {
            return _minValue;
        }
        set {
            if (value < 0) value = 0;
            _minValue = value;
        }
    }
    public bool IsEmpty => CurrentValue <= MinValue;
    public bool IsFull => CurrentValue >= MaxValue;

    // Constructors
    protected BaseStat(float initialMaxValue) {
        MaxValue = initialMaxValue;
        MinValue = 0;
        CurrentValue = MaxValue;
    }

    protected BaseStat(float initialMaxValue, float initialMinValue) {
        MaxValue = initialMaxValue;
        MinValue = initialMinValue;
        CurrentValue = MaxValue;
    }

    // Public Methods
    public float GetRatio() {
        return CurrentValue / MaxValue;
    }

    // Overriden Methods
    public override string ToString() {
        return $"{CurrentValue.ToString("F2")}/{MaxValue.ToString("F2")}";
    }

}
