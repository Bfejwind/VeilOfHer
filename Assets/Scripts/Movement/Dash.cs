using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    FirstPersonController moveScript;
    private bool isDashing;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown = 3.0f;
    private bool canDash = true;
    private KeyCode dashKey = KeyCode.LeftShift;
    void Start()
    {
        moveScript = GetComponent<FirstPersonController>();
    }
    void Update()
    {
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            if (!isDashing)
            {
                StartCoroutine(Dashing());
            }
        }
    }
    private IEnumerator Dashing()
    {
        isDashing = true;
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            moveScript._controller.Move(moveScript.inputDirection * (dashSpeed * Time.deltaTime));
            yield return null;
        }
        isDashing = false;
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
