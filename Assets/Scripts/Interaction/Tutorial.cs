using UnityEngine;
using TMPro;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial Text")]
    [SerializeField]
    public TMP_Text tutorialText;

    bool basicTutorial = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!basicTutorial)
        {
            tutorialText.gameObject.SetActive(true);
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        tutorialText.text = "Welcome to the game! \n \n Here is a quick tutorial: \n \n Use <b>W A S D</b> to move around.";
    }

    public void MovementTutorial()
    {
            tutorialText.text = "Great! Now try sprinting towards the waypoint by holding <b>Shift</b>. \n \n Use <b>Space</b> to jump over obstacles."; 
    }

    public void InteractionTutorial()
    {
        tutorialText.text = "Well done! Now, try pressing <b>E</b> to interact with the object.";
        tutorialText.gameObject.SetActive(false);
    }

    public void CombatTutorial()
    {
        tutorialText.text = "Now, let's learn about combat. Use the left mouse button to attack.";

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(WaitForSeconds(3f));
            tutorialText.text = "Great! Now, try using your special ability by pressing the right mouse button.";
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(WaitForSeconds(3f));
                tutorialText.text = "Excellent! You've completed the combat tutorial.";
                StartCoroutine(WaitForSeconds(3f));
            }
        }
    }



    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
