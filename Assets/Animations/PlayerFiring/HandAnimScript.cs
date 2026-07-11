using UnityEngine;

public class HandAnimScript : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    //Firing
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
    //Ability
    public void PlayHandsAbilityStart()
    {
        animator.SetTrigger("AbilityStart");
        animator.SetBool("AbilityLoaded", true);
    }
    public void PlayHandsAbilityShoot()
    {
        animator.SetTrigger("AbilityFire");
        animator.SetBool("AbilityLoaded", false);
    }
}
