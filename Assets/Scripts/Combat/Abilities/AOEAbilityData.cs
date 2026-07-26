using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AOE Ability")]
public class AOEAbilityData : AbilityData
{
    [Header("AOE Settings")]
    public GameObject aoeImpactPrefab;
    
    public float aoeRadius;
    public float aoeBaseDamage;
    
    public LayerMask targetLayerMask;
}
