using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pmScript;
    private Vector3 delayedForceToApply;
    
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
        pmScript = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            Debug.Log("Dashing");
            Dash();
        }
        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pmScript.dashing = true;
        if (pmScript.moveDirection == Vector3.zero)
        {
             Vector3 forceToApply = orientation.forward * dashForce + orientation.forward * dashUpwardsForce;
             delayedForceToApply = forceToApply;
        }
        else
        {
            Vector3 forceToApply = pmScript.moveDirection * dashForce + pmScript.moveDirection * dashUpwardsForce;
            delayedForceToApply = forceToApply;
        }
        Invoke(nameof(DelayedDashForce),0.025f);

        Invoke(nameof(ResetDash),dashDuration);
    }
    private void ResetDash()
    {
        pmScript.dashing = false;
    } 
    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }
}
