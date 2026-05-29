using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private FirstPersonMovement pmScript;
    
    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardsForce;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.LeftShift;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pmScript = GetComponent<FirstPersonMovement>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            Dash();
        }
    }

    private void Dash()
    {
        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardsForce;
        rb.AddForce(forceToApply,ForceMode.Impulse);

        Invoke(nameof(ResetDash),dashDuration);
    }
    private void ResetDash()
    {
        
    } 
}
