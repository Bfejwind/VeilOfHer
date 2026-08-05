using System;
using System.Collections;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    FirstPersonController moveScript;
    private float currentStamina;
    public float maxStamina = 100.0f;
    public float staminaCost = 25.0f;
    public float staminaRefund = 15.0f;
    public float staminaRegenRate = 10.0f;
    private Coroutine regenCoroutine;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float sliderFillDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI staminaText;
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
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        UpdateStaminaSlider();
    }
    void Update()
    {
        FillSliderGradual();
        if (currentStamina >= staminaCost)
        {
            canDash = true;
        }
        else
        {
            canDash = false;
        }
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
        currentStamina -= staminaCost;
        float startTime = Time.time;
        playerAudio.PlayDashSFX();
        while (Time.time < startTime + dashTime)
        {
            moveScript._controller.Move(moveScript.inputDirection * (dashSpeed * Time.deltaTime));
            yield return null;
        }
        isDashing = false;
        if (dashThrough)
        {
            currentStamina += staminaRefund;
        }
        UpdateStaminaSlider();
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }
        regenCoroutine = StartCoroutine(RegenStamina());
    }
    // private IEnumerator Dashing()
    // {
    //     isDashing = true;
    //     dashThrough = false;
    //     float startTime = Time.time;
    //     playerAudio.PlayDashSFX();
    //     while (Time.time < startTime + dashTime)
    //     {
    //         moveScript._controller.Move(moveScript.inputDirection * (dashSpeed * Time.deltaTime));
    //         yield return null;
    //     }
    //     isDashing = false;
    //     canDash = false;
    //     if (dashThrough)
    //     {
    //         yield return new WaitForSeconds(reducedCD);
    //     }
    //     else
    //     {
    //         yield return new WaitForSeconds(dashCooldown);
    //     }
    //     canDash = true;
    // }
    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2f);

        while (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate;
            UpdateStaminaSlider();
            yield return new WaitForSeconds(2.0f);
        }
        regenCoroutine = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && isDashing)
        {
            dashThrough = true;
            playerAudio.PlayDashThroughSFX();

        }
    }
    private void UpdateStaminaSlider()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaText.text = currentStamina + "/" + maxStamina;
        //staminaSlider.value = currentStamina;
    }
    private void FillSliderGradual()
    {
        float speed = Mathf.Abs(currentStamina - staminaSlider.value) / sliderFillDuration;
        staminaSlider.value = Mathf.MoveTowards(staminaSlider.value, currentStamina, speed * Time.deltaTime);
    }
}
