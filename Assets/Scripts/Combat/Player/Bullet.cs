using UnityEngine;

public class Bullet : MonoBehaviour
{
    //[Header("Layers")]
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            //print("hit" + other.gameObject.name);
            GameManager.Instance.ChangeLightRadius(2.0f);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Environment"))
        {
            //print("hit" + other.gameObject.name);
            Destroy(gameObject);
        }
    }
}
