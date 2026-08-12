using UnityEngine;

public class StairBlock : MonoBehaviour
{
    [SerializeField] private GameObject stairBlock;
    private void Start()
    {
        if (GameManager.Instance.lvl2bComplete)
        {
            stairBlock.SetActive(false);
        }
    }
}
