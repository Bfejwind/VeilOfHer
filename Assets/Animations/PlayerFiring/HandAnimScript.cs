using UnityEngine;

public class HandAnimScript : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayHandsFireStart()
    {
        animator.SetTrigger("Fire");
        animator.SetBool("isFiring", true);
    }
    public void PlayHandsFireStop()
    {
        animator.SetBool("isFiring", false);
    }
    public void PlayReload()
    {
        animator.SetTrigger("Reload");
    }
    public void PlayHandsAbilityStart()
    {
        animator.SetTrigger("AbilityStart");
        animator.SetBool("AbilityLoaded", true);
    }
    public void PlayHandsAbilityShoot()
    {
        animator.SetTrigger("AbilityFire");
    }
    public void PlayHandsAbilityStop()
    {
        animator.SetBool("AbilityLoaded", false);
    }
}
