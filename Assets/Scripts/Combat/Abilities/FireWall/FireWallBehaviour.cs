using UnityEngine;

public class FireWallBehaviour : MonoBehaviour
{
    public float fireWallCurrentWidth;
    public float fireWallDuration;
    public float fireWallMaxHP;
    public float fireWallCurrentHP;
    void Start()
    {
        fireWallCurrentHP = fireWallMaxHP;
        Destroy(gameObject,fireWallDuration);
    }
}
