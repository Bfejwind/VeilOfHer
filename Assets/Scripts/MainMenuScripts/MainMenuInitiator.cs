using UnityEngine;

public class MainMenuInitiator : MonoBehaviour
{
    public void EnterGame()
    {
        GameManager.Instance.GMEnterGame();
    }
    public void EnterArena()
    {
        GameManager.Instance.GMEnterArena();
    }
    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}
