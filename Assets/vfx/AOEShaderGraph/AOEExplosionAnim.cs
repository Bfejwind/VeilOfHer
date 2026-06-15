using UnityEngine;
using UnityEngine.VFX;

public class AOEExplosionAnim : MonoBehaviour
{
    [SerializeField]private VisualEffect sparkParticles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sparkParticles.Stop();
    }
    public void StartExplosion()
    {
        //Debug.Log("Sparks playing");
        sparkParticles.Play();
    }
    public void DeleteExplosion()
    {
        Destroy(gameObject);
    }
}
