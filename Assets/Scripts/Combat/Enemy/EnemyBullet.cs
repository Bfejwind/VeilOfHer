using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //[Header("Layers")]
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            //print("hit" + other.gameObject.name);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            //print("hit" + other.gameObject.name);
            Destroy(gameObject);
        }
    }
}
