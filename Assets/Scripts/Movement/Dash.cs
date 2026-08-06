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
    [Header("DashFX")]
    [SerializeField] private Material dashFXMaterial;
    [SerializeField] private float dashEffectFadeTime = 0.15f;

    private static readonly int IntensityID = Shader.PropertyToID("_Intensity");
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
        dashFXMaterial.SetFloat(IntensityID, 0f);
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
        float elapsed = 0f;
        playerAudio.PlayDashSFX();
        while (elapsed < dashTime)
        {
            elapsed += Time.deltaTime;
            float intensity = 1f;
            if (elapsed < dashSpeed * 0.2f)
            {
                intensity = elapsed/(dashTime *0.2f);
            }
            else if (elapsed > dashTime * 0.8f)
            {
                intensity = 1f - ((elapsed - dashTime * 0.8f) / (dashTime * 0.2f));
            }
            dashFXMaterial.SetFloat(IntensityID, intensity);
            moveScript._controller.Move(moveScript.inputDirection * (dashSpeed * Time.deltaTime));
            yield return null;
        }
        dashFXMaterial.SetFloat(IntensityID, 0f);
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
    //     //DashFX
    //     if (dashEffectCoroutine != null)
    //     {
    //         StopCoroutine(dashEffectCoroutine);
    //     }
    //     dashEffectCoroutine = StartCoroutine(DashScreenEffect());
    //     currentStamina -= staminaCost;
    //     float startTime = Time.time;
    //     playerAudio.PlayDashSFX();
    //     while (Time.time < startTime + dashTime)
    //     {
    //         moveScript._controller.Move(moveScript.inputDirection * (dashSpeed * Time.deltaTime));
    //         yield return null;
    //     }
    //     isDashing = false;
    //     if (dashThrough)
    //     {
    //         currentStamina += staminaRefund;
    //     }
    //     UpdateStaminaSlider();
    //     if (regenCoroutine != null)
    //     {
    //         StopCoroutine(regenCoroutine);
    //     }
    //     regenCoroutine = StartCoroutine(RegenStamina());
    // }
    // private IEnumerator DashScreenEffect()
    // {
    //     float timer = 0f;
    //     //Fade in
    //     while (timer < dashEffectFadeTime)
    //     {
    //         timer += Time.deltaTime;
    //         float t = timer/dashEffectFadeTime;
    //         dashFXMaterial.SetFloat(IntensityID, t);
    //         yield return null;
    //     }
    //     dashFXMaterial.SetFloat(IntensityID, 1f);
    //     yield return new WaitForSeconds(dashTime);
    //     timer = 0f;
    //     //Fade out
    //     while (timer < dashEffectFadeTime)
    //     {
    //         timer += Time.deltaTime;
    //         float t = 1f - (timer/dashEffectFadeTime);
    //         dashFXMaterial.SetFloat(IntensityID,t);
    //         yield return null;
    //     }
    //     dashFXMaterial.SetFloat(IntensityID, 0f);
    // }
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
            yield return new WaitForSeconds(1.5f);
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
