using UnityEngine;

public class IndicatorBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CommandCaster command;
    private float duration = 5.0f;
    private AbilityData currentAbility;
    private void Awake()
    {
        if (command == null)
        {
            command = GetComponentInParent<CommandCaster>();
        }
    }
    private void Start()
    {
        currentAbility = command.currentAbility;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment") || collision.gameObject.CompareTag("Enemy"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 impactPoint = contact.point;
            command.CastAbility(currentAbility, impactPoint);
            Destroy(gameObject);
        }
    }

}
