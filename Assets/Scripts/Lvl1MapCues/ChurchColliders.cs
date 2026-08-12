using UnityEngine;

public class ChurchColliders : MonoBehaviour
{
    [SerializeField] private GameObject churchBlock;
    [SerializeField] private GameObject throneBlock;
    [SerializeField] private GameObject meleeBoss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerBehaviour player))
        {
            churchBlock.SetActive(true);
            throneBlock.SetActive(true);
            meleeBoss.SetActive(true);
        }
    }
}
