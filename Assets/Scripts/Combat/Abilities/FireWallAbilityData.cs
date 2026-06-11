using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/FireWall")]
public class FireWallAbilityData : AbilityData
{
    [Header("Fire Wall Settings")]
    public GameObject fireWallPrefab;
    public float fireWallWidth;
    public float fireWallDuration;
    public float fireWallDamagePerSecond;
    public bool reflectProjectiles;
}
