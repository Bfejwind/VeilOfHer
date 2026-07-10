using UnityEngine;

public class HandAnimScript : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayHandsIdle()
    {
        animator.Play("HandsIdleBetter");
    }
    public void PlayHandsFireStart()
    {
        animator.Play("HandsFireStart");
    }
    public void PlayHandsFiring()
    {
        animator.Play("HandsFiring");
    }
    public void PlayHandsFireStop()
    {
        animator.Play("HandsFireStop");
    }
    public void PlayReload()
    {
        animator.Play("Reload");
    }
    public void PlayHandsAbilityStart()
    {
        animator.Play("HandsAbilityStart");
    }
    public void PlayHandsAbilityIdle()
    {
        animator.Play("HandsAbilityIdle");
    }
    public void PlayHandsAbilityShoot()
    {
        animator.Play("HandsAbilityShoot");
    }
    public void PlayHandsAbilityStop()
    {
        animator.Play("HandsAbilityStop");
    }
}
