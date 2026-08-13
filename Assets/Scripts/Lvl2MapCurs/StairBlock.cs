using UnityEngine;

public class StairBlock : MonoBehaviour
{
    [SerializeField] private GameObject stairBlock;
    private void Start()
    {
        GameManager.Instance.Playlevel123BGM();
        if (GameManager.Instance.lvl2bComplete)
        {
            stairBlock.SetActive(false);
        }
    }
}
