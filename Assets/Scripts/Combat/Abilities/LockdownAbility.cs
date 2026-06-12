using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LockdownAbility : MonoBehaviour
{
    public float radius;
    public float duration;
    private List<EnemyBehaviour>affectedEnemies = new();
    void Start()
    {
        Destroy(gameObject, duration);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBehaviour enemy))
        {
            if (!affectedEnemies.Contains(enemy))
            {
                affectedEnemies.Add(enemy);
                enemy.ApplyControl();
            }
        }
    }
    private void OnDestroy()
    {
        foreach (EnemyBehaviour enemy in affectedEnemies)
        {
            if (enemy != null)
            {
                enemy.RemoveControl();
            }
        }
    }
}
