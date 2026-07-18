using UnityEngine;

public class MeleeWarningBehaviour : MonoBehaviour
{
    private float duration = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, duration);
    }

}
