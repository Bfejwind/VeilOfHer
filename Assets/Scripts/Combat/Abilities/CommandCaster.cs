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
    [Space]
    [Header("Modifiers")]
    [SerializeField] private float aoeDamageMod;
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
    private AbilityData currentAbility;
    private Coroutine slowMoCoroutine;
    private void Awake()
    {
        playerStats = GetComponent<PlayerBehaviour>();
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
        if (currentAbility != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            CastAbility(currentAbility);
            EndTargeting();
            currentAbility = null;
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
            currentCharges--;
            if (cdTracker != null)
            {
                cdTracker.CMDChargesTracker(currentCharges.ToString());
                StartCoroutine(CooldownIconRoutine());
            }
            currentAbility = ability;
            slowMoCoroutine = StartCoroutine(SlowTime());
            //CastAbility(ability);
            StartCoroutine(RechargeCharge());
        }
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
    private void CastAbility(AbilityData ability)
    {
        Vector3 targetPoint = GetTargetPoint();
        if (ability is ControlAbilityData control)
        {
            GameObject bubble = Instantiate(control.bubblePrefab, targetPoint, Quaternion.identity);
            var bubbleLogic = bubble.GetComponent<LockdownAbility>(); 
            bubbleLogic.radius = control.bubbleRadius;
            bubbleLogic.duration = control.bubbleDuration;
            return;
        }
        if (ability is AOEAbilityData aoe)
        {
            GameObject aoePrefab = Instantiate(aoe.aoeImpactPrefab, targetPoint,Quaternion.identity);
            var aoeAnim = aoePrefab.GetComponent<AOEExplosionAnim>();
            var aoeLogic = aoePrefab.GetComponent<AOEBehaviour>();
            float aoeFinalDamage = (aoe.aoeBaseDamage + aoeDamageMod) * playerStats.abilityDamageMultiplier;
            aoeLogic.AOEDamageCalc(aoeFinalDamage);
            //Debug.Log("AOE activated");
            aoeAnim.StartExplosion();
            return;
        }
        if (ability is FireWallAbilityData firewall)
        {
            Vector3 directionToFace = playerCamera.transform.position - targetPoint;
            directionToFace.y = 0;
            Quaternion rotation = Quaternion.LookRotation(directionToFace);
            GameObject firewallObject = Instantiate(firewall.fireWallPrefab, targetPoint,rotation);
            var firewallLogic = firewallObject.GetComponent<FireWallBehaviour>();
            firewallLogic.fireWallCurrentWidth = firewall.fireWallBaseWidth;
            firewallLogic.fireWallDuration = firewall.fireWallBaseDuration;
            firewallLogic.fireWallMaxHP = firewall.fireWallBaseMaxHP;
            return;

        }
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
    private IEnumerator SlowTime()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.5f);
        EndTargeting();
    }
    private void EndTargeting()
    {
        if (slowMoCoroutine != null)
        {
            StopCoroutine(SlowTime());
            slowMoCoroutine = null;
        }
        Time.timeScale = 1.0f;
    }
}
