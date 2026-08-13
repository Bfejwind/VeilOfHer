using UnityEngine;

public class DedCanvasScript : MonoBehaviour
{
    public void RetryScene()
    {
        GameManager.Instance.Retry();
    }
    public void Quit()
    {
        GameManager.Instance.Quit();
    }
}
