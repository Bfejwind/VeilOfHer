using UnityEngine;

public class HealOrbsLoot : MonoBehaviour
{
    [SerializeField] private int orbNumber;
    [SerializeField] private GameObject healOrbPrefab;
    [SerializeField] private float spawnRadius = 1.0f;
    [SerializeField] private float spawnHeight = 0.5f;
    [SerializeField] private float spawnForce = 5.0f;
    private void Start()
    {
        orbNumber = Random.Range(1, 4);
    }
    public void GenerateHealOrbs()
    {
        for (int i = 0; i < orbNumber; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), spawnHeight, Random.Range(-spawnRadius, spawnRadius));
            GameObject healOrb = Instantiate(healOrbPrefab, randomPosition, Quaternion.identity);
            healOrb.GetComponent<Rigidbody>().AddForce(Vector3.up * spawnForce, ForceMode.Impulse);
        }
    }
}
