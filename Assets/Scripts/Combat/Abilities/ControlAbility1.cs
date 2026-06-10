using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class ControlAbility1 : MonoBehaviour
{
    [SerializeField] private GameObject ControlZonePrefab;
    public Vector3 targetPoint;
    public bool abilityInRange;
    public float rayLength = 10f;
    public Camera mainCamera;
    //public KeyCode ability1Key = KeyCode.E;
    public LayerMask hitMask;

    // Update is called once per frame
    void Update()
    {
        // RaycastHit hit;

        // // Always update target while in targeting mode
        // if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, rayLength, hitMask))
        // {
        //     targetPoint = hit.point;
        //     abilityInRange = true;
        //     if (Input.GetKeyUp(ability1Key))
        //     {
        //         // Activate the ability
        //         Instantiate(ControlZonePrefab, targetPoint, Quaternion.identity);
        //     }
        // }
        // else
        // {
        //     abilityInRange = false;
        // }
    }
    public void ActivateAbility()
    {
        RaycastHit hit;

        // Always update target while in targeting mode
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, rayLength, hitMask))
        {
            targetPoint = hit.point;
            abilityInRange = true;
            Instantiate(ControlZonePrefab, targetPoint, Quaternion.identity);
        }
        else
        {
            abilityInRange = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // DrawRay(Start point, Direction vector * distance)
        Gizmos.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * rayLength);
    }
}
