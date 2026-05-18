using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //[Header("Layers")]
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            //print("hit" + collision.gameObject.name);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            //print("hit" + collision.gameObject.name);
            Destroy(gameObject);
        }
    }
}
