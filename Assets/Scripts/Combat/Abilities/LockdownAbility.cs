using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LockdownAbility : MonoBehaviour
{
    public float radius;
    public float duration;
    [SerializeField]private List<EnemyBehaviour>affectedEnemies = new();
    [SerializeField]private List<FinalBossMovement>affectedBosses = new();
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
        if (other.TryGetComponent(out FinalBossMovement boss))
        {
            if (!affectedBosses.Contains(boss))
            {
                affectedBosses.Add(boss);
                boss.ApplyControl();
                boss.nerfDuration = duration;
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
        foreach (FinalBossMovement boss in affectedBosses)
        {
            if (boss != null)
            {
                boss.RemoveControl();
            }
        }
    }
}
