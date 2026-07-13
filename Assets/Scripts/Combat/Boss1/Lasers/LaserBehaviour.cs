using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    [SerializeField] private float damage = 20.0f;
    //[Header("Layers")]
    void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent(out PlayerHealth playerHP))
        {
            //print("hit" + other.gameObject.name);
            playerHP.TakeDamage(damage);
        }
    }
}
