using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/FireWall")]
public class FireWallAbilityData : AbilityData
{
    [Header("Fire Wall Settings")]
    public GameObject fireWallPrefab;
    public float fireWallBaseWidth;
    public float fireWallBaseDuration;
    public float fireWallBaseMaxHP;
}
