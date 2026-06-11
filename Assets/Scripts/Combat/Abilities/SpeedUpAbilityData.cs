using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SpeedUp")]
public class SpeedUpAbilityData : AbilityData
{
    [Header("Buff Settings")]
    public float speedMultiplier;
    public float buffDuration;
    public GameObject buffEffectPrefab;
}
