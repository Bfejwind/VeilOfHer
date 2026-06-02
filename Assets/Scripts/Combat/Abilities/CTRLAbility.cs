using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu]
public class CTRLAbility : Ability
{
    [SerializeField] private GameObject ControlTriggerZonePrefab;
    public override void Activate(GameObject parent)
    {
        AbilityHolder holder = parent.GetComponent<AbilityHolder>();
        if (holder != null)
        {
            Vector3 targetPoint = holder.targetPoint;
            Instantiate(ControlTriggerZonePrefab, targetPoint, Quaternion.identity);
        }
    }
}
