using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Delete Ability")]
public class DeleteAbilityData : AbilityData
{
    [Header("Spawn Settings")]
    public GameObject spawnObjectPrefab;
    public int spawnCount;
    [Header("Projectiles")]
    public GameObject projectilePrefab;
    public float fireRate;
    [Header("General Settings")]
    public float deleteDuration;
}
