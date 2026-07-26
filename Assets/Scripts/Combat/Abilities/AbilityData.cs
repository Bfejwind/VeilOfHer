using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    [Header("General")]
    public string abilityName;
    [TextArea(3, 10)]
    public string description;
    public GameObject abilityIndicator;
    [Space]
    [Header("Visuals")]
    public Sprite icon;
    public GameObject visualEffectPrefab;
    [Space]
    [Header("Range")]
    public float range;
}
