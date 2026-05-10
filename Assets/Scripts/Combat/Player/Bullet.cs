using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" + collision.gameObject.name);
            GameManager.Instance.ChangeLightRadius(2.0f);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Environment"))
        {
            print("hit" + collision.gameObject.name);
            Destroy(gameObject);
        }
    }
}
