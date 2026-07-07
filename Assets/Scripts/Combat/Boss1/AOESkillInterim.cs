using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AOESkillInterim : MonoBehaviour
{
    [SerializeField] private GameObject aoePrefab;
    [SerializeField] private float dissolveDuration = 4.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DissolveAOE());
    }
    private IEnumerator DissolveAOE()
    {
        float elapsed = 0f;
        Debug.Log("Playing AOE Particle Systems");
        GameObject aoeInstance = Instantiate(aoePrefab, transform.position, Quaternion.identity);
        ParticleSystem[] particleSystems = aoeInstance.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(3.0f); // Wait for 1 second before starting the dissolve effect
        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float value = Mathf.Lerp(0f, 1f, elapsed / dissolveDuration);
            foreach (ParticleSystem ps in aoeInstance.GetComponentsInChildren<ParticleSystem>())
            {
                Material mat = ps.GetComponent<Renderer>().material;
                mat.SetFloat("_dissovel_amount", value);
            }
            yield return null;
        }
    }

}
