using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    float cooldownTimer;
    float activeTimer;
    private bool abilityInRange;
    public float rayLength = 10f;
    public Vector3 targetPoint;

    enum AbilityState
    {
        Targeting,
        Ready,
        Active,
        Cooldown
    }
    AbilityState state = AbilityState.Ready;
    public KeyCode ability1Key;
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            targetPoint = hit.point;
            abilityInRange = true;
        }
        else
        {
            abilityInRange = false;
        }
        switch (state)
        {
            case AbilityState.Targeting:
                if (Input.GetKeyDown(ability1Key))
                {
                    state = AbilityState.Ready;
                }
                break;
            case AbilityState.Ready:
                if (Input.GetKeyUp(ability1Key))
                {
                    if (abilityInRange)
                    {
                        ability.Activate(gameObject);
                        state = AbilityState.Active;
                        activeTimer = ability.activeTime;
                    }
                }
            break;
            case AbilityState.Active:
                if (activeTimer > 0)
                {
                    activeTimer -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Cooldown;
                    cooldownTimer = ability.cooldownTime;
                }
            break;
            case AbilityState.Cooldown:
                if (cooldownTimer > 0)
                {
                    cooldownTimer -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Ready;
                }
            break;
        }
    }

    // Drawn all the time in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // DrawRay(Start point, Direction vector * distance)
        Gizmos.DrawRay(transform.position, transform.forward * rayLength);
    }
}
