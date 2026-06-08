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
    private KeyCode dashKey = KeyCode.LeftShift;
    void Start()
    {
        moveScript = GetComponent<FirstPersonController>();
    }
    void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            StartCoroutine(Dashing());
        }
    }
    private IEnumerator Dashing()
    {
        isDashing = true;
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            Debug.Log("Dashing");
            moveScript._controller.Move(moveScript.inputDirection * (dashSpeed * Time.deltaTime));
            yield return null;
        }
        isDashing = false;
    }
}
