using StarterAssets;
using UnityEngine;

public class ShortTutScript : MonoBehaviour
{
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private Weapon playerShoot;
    [SerializeField] private GameObject[] tutorialScreens;
    private int currentTutorial = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController.canMove = false;
        playerShoot.readyToShoot = false;
        currentTutorial = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            NextScreen();
        }
    }
    private void NextScreen()
    {
        if (tutorialScreens.Length > 0)
        {
            tutorialScreens[currentTutorial].SetActive(false);

            currentTutorial++;

            if (currentTutorial < tutorialScreens.Length)
            {
                tutorialScreens[currentTutorial].SetActive(true);
            }
            else
            {
                TutFinished();
            }
        }
    }
    private void TutFinished()
    {
        playerController.canMove = true;
        playerShoot.readyToShoot = true;
        gameObject.SetActive(false);
    }
}
