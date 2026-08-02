using System;
using System.Collections;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    FirstPersonController moveScript;
    private bool isDashing;
    public bool dashThrough;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown = 3.0f;
    public float reducedCD = 0.3f;
    private bool canDash = true;
    private KeyCode dashKey = KeyCode.LeftShift;
    //Invulnerability
    private PlayerHealth playerHealth;
    private float invulnerableDuration;
    [Header("Audio")]
    [SerializeField] private PlayerAudio playerAudio;
    void Awake()
    {
        if (playerAudio == null)
        {
            playerAudio = GetComponent<PlayerAudio>();
        }
    }
    void Start()
    {
        moveScript = GetComponent<FirstPersonController>();
        playerHealth = GetComponent<PlayerHealth>();
        invulnerableDuration = dashTime;
    }
    void Update()
    {
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            if (!isDashing)
            {
                StartCoroutine(Dashing());
                StartCoroutine(playerHealth.DashInvulnerability(invulnerableDuration));
            }
        }
    }
    private IEnumerator Dashing()
    {
        isDashing = true;
        dashThrough = false;
        float startTime = Time.time;
        playerAudio.PlayDashSFX();
        while (Time.time < startTime + dashTime)
        {
            moveScript._controller.Move(moveScript.inputDirection * (dashSpeed * Time.deltaTime));
            yield return null;
        }
        isDashing = false;
        canDash = false;
        if (dashThrough)
        {
            yield return new WaitForSeconds(reducedCD);
        }
        else
        {
            yield return new WaitForSeconds(dashCooldown);
        }
        canDash = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && isDashing)
        {
            dashThrough = true;
            playerAudio.PlayDashThroughSFX();

        }
    }
}
