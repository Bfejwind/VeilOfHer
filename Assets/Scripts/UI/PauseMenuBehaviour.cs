using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause UI")]
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused;

    private void Start()
    {
        // Ensure the game starts normally.
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenu == null)
        {
            Debug.LogWarning("Pause Menu has not been assigned.");
            return;
        }

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Show and unlock the cursor for menu interaction.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;

        // Lock the cursor again for gameplay.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}