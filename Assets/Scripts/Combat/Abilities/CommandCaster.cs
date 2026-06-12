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
    private void Awake()
    {
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
            CastAbility(ability);
            StartCoroutine(RechargeCharge());
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
            var aoeLogic = aoePrefab.GetComponent<AOEExplosionAnim>();
            Debug.Log("AOE activated");
            aoeLogic.StartExplosion();
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
}
