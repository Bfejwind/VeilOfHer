using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Control Ability")]
public class ControlAbilityData : AbilityData
{
    [Header("Bubble")]
    public GameObject bubblePrefab;

    public float bubbleRadius;
    public float bubbleDuration;

    [Header("Effects")]
    public bool stopMovement;
    public bool stopAttacking;

}
