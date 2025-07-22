using UnityEngine;

public abstract class BaseStatusEffectData : ScriptableObject {
    [Tooltip("ID of Status Effect")]
    public uint ID = 0;

    [Tooltip("Display name of Status Effect")]
    public string EffectName = "New Status Effect";

    [Tooltip("Description of Status Effect")]
    [TextArea(3, 5)]
    public string Description = "A brief description of what this status effect does";

    [Tooltip("Duration of Status Effect is seconds. (0 = infinite)")]
    public float Duration = 5f;

    public enum StatusEffectType {
        Buff,
        Neutral,
        Debuff
    }

    [Tooltip("Type of the Status Effect")]
    public StatusEffectType EffectType = StatusEffectType.Neutral;

    public abstract BaseStatusEffectLogic CreateStatusEffectLogicInstance();
    
}
