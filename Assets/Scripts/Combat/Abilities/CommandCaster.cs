using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommandCaster : MonoBehaviour
{
    [Header("References Player")]
    [SerializeField]private Camera playerCamera;
    [SerializeField]private float maxRange = 20f;
    [SerializeField]private LayerMask aimMask;
    [SerializeField] private Transform abilityIndicatorOrigin;
    //Animations
    [SerializeField] private HandAnimScript handAnim;

    [Header("Charge System")]
    public int maxCharges = 2;
    public int currentCharges;
    public float rechargeTime = 10f;
    private float rechargeCountdown;
    public float RechargeTimer => rechargeCountdown;
    [SerializeField] private CooldownTracker cdTracker;
    [Space]
    [Header("Player Stats")]
    private PlayerBehaviour playerStats;
    public bool abilityLoaded;
    [Space]
    [Header("Modifiers")]
    [SerializeField] private float aoeDamageMod;
    [SerializeField] private float aoeCalldownDelay = 1.0f;
    [Space]
    [Header("Ability Mapping")]
    private string commandInput;
    [SerializeField] private List<CommandPair> commandList;
    private Dictionary<string, AbilityData> commandLookup;
    [System.Serializable]
    public class CommandPair
    {
        public string command;
        public AbilityData ability;
    }
    //Targeting
    public AbilityData currentAbility;
    private GameObject currentIndicator;
    private Rigidbody indicatorRB;
    [SerializeField] private float indicatorLaunchForce = 10.0f;
    public Vector3 impactPoint;
    private Coroutine slowMoCoroutine;
    //Delay weapon Firing
    [SerializeField] private float scuffedWeaponDelay = 0.2f;
    private void Awake()
    {
        playerStats = GetComponent<PlayerBehaviour>();
        handAnim = GetComponentInChildren<HandAnimScript>();
        commandLookup = new Dictionary<string, AbilityData>();
        foreach (var pair in commandList)
        {
            commandLookup[pair.command] = pair.ability; //Add [pair.command.ToLower()] if you want to ignore case sensitivity
        }
    }
    void Start()
    {
        currentCharges = maxCharges;
    }
    void Update()
    {
        if (currentAbility != null)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Shoot Ball Indicator
                LaunchIndicator();
                StartCoroutine(ScuffedAbilityLoad());
                //Turn on weapon after ability is fired
                //EndTargeting();
                currentAbility = null;
            }
        }
    }
    public void ExecuteCommand(string input)
    {
        if (currentCharges <= 0)
        {
            Debug.Log("No charges available!");
            return;
        }

        //Add input = input.ToLower() if you want to ignore case sensitivity
        else if (commandLookup.TryGetValue(input, out AbilityData ability))
        {
            abilityLoaded = true;
            currentCharges--;
            if (cdTracker != null)
            {
                cdTracker.CMDChargesTracker(currentCharges.ToString());
                StartCoroutine(CooldownIconRoutine());
            }
            currentAbility = ability;
            //Ability Distinguishing Indicators
            if (currentAbility is ControlAbilityData control)
            {
                currentIndicator = Instantiate(control.abilityIndicator,abilityIndicatorOrigin.position,Quaternion.identity, transform);
                
            }
            if (currentAbility is AOEAbilityData aoe)
            {
                currentIndicator = Instantiate(aoe.abilityIndicator,abilityIndicatorOrigin.position,Quaternion.identity, transform);
            }
            if (currentAbility is FireWallAbilityData firewall)
            {
                currentIndicator = Instantiate(firewall.abilityIndicator,abilityIndicatorOrigin.position,Quaternion.identity, transform);
            }
            StartCoroutine(RechargeCharge());
            //Animations
            handAnim.PlayHandsAbilityStart();
        }
    }
    private void LaunchIndicator()
    {
        if (currentIndicator != null && indicatorRB == null)
        {
            handAnim.PlayHandsAbilityShoot();
            Vector3 targetPoint = GetTargetPoint();
            Vector3 launchDirection = (targetPoint - abilityIndicatorOrigin.position).normalized;
            indicatorRB = currentIndicator.GetComponent<Rigidbody>();
            indicatorRB.AddForce(launchDirection * indicatorLaunchForce,ForceMode.Impulse);
            currentIndicator.transform.SetParent(null);
        }
        indicatorRB = null;
        currentIndicator = null;
    }
    private IEnumerator CooldownIconRoutine()
    {
        float timer = rechargeTime;

        while (timer > 0)
        {
            float progress = timer / rechargeTime;

            if (cdTracker != null)
            {
                cdTracker.SetCMDCooldownFill(progress);

                cdTracker.CMDCooldownTracker(
                    Mathf.CeilToInt(timer).ToString()
                );
            }

            timer -= Time.unscaledDeltaTime;
            yield return null;
        }

        if (cdTracker != null)
        {
            cdTracker.SetCMDCooldownFill(0f);
            cdTracker.CMDCooldownTracker("");
        }
    }
    private IEnumerator RechargeCharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        currentCharges = Mathf.Min(currentCharges + 1, maxCharges);
    }
    public void CastAbility(AbilityData ability,Vector3 targetPoint)
    {
        if (ability is ControlAbilityData control)
        {
            Lockdown(control, targetPoint);
            return;
        }
        if (ability is AOEAbilityData aoe)
        {
            StartCoroutine(AOEStrike(aoe, targetPoint));
            return;
        }
        if (ability is FireWallAbilityData firewall)
        {
            Firewall(firewall, targetPoint);
            return;
        }
    }
    private void Lockdown(ControlAbilityData control, Vector3 targetPoint)
    {
        GameObject bubble = Instantiate(control.bubblePrefab, targetPoint, Quaternion.identity);
        var bubbleLogic = bubble.GetComponent<LockdownAbility>(); 
        bubbleLogic.radius = control.bubbleRadius;
        bubbleLogic.duration = control.bubbleDuration;
    }
    private IEnumerator AOEStrike(AOEAbilityData aoe, Vector3 targetPoint)
    {
        yield return new WaitForSeconds(aoeCalldownDelay);
        GameObject aoePrefab = Instantiate(aoe.aoeImpactPrefab, targetPoint, Quaternion.identity);
        var aoeAnim = aoePrefab.GetComponent<AOEExplosionAnim>();
        var aoeLogic = aoePrefab.GetComponent<AOEBehaviour>();
        float aoeFinalDamage = (aoe.aoeBaseDamage + aoeDamageMod) * playerStats.abilityDamageMultiplier;
        aoeLogic.AOEDamageCalc(aoeFinalDamage);
        //Debug.Log("AOE activated");
        aoeAnim.StartExplosion();
    }
    private void Firewall(FireWallAbilityData firewall, Vector3 targetPoint)
    {
        Vector3 directionToFace = playerCamera.transform.position - targetPoint;
        directionToFace.y = 0;
        Quaternion rotation = Quaternion.LookRotation(directionToFace);
        GameObject firewallObject = Instantiate(firewall.fireWallPrefab, targetPoint,rotation);
        var firewallLogic = firewallObject.GetComponent<FireWallBehaviour>();
        firewallLogic.fireWallCurrentWidth = firewall.fireWallBaseWidth;
        firewallLogic.fireWallDuration = firewall.fireWallBaseDuration;
        firewallLogic.fireWallMaxHP = firewall.fireWallBaseMaxHP;
    }
    private Vector3 GetTargetPoint()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRange,aimMask))
        {
            return hit.point;
        }
        return ray.origin + ray.direction * maxRange;
    }
    // private IEnumerator SlowTime()
    // {
    //     Time.timeScale = 0.2f;
    //     yield return new WaitForSeconds(0.5f);
    //     EndTargeting();
    // }
    // private void EndTargeting()
    // {
    //     if (slowMoCoroutine != null)
    //     {
    //         StopCoroutine(SlowTime());
    //         slowMoCoroutine = null;
    //     }
    //     Time.timeScale = 1.0f;
    // }
    private IEnumerator ScuffedAbilityLoad()
    {
        yield return new WaitForSeconds(scuffedWeaponDelay);
        abilityLoaded = false;
    }
}
