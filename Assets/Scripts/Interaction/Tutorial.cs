using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial Text")]
    [SerializeField]
    public TMP_Text tutorialText;

    [Header("Tutorial Events")]
    [SerializeField] private UnityEvent completedObjective;

    bool basicTutorial = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!basicTutorial)
        {
            tutorialText.gameObject.SetActive(true);
            StartTutorial();
        }
        else
        {
            tutorialText.gameObject.SetActive(true);
            CombatTutorial();
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
        tutorialText.text = "Well done! Now, walk up to the object and press <b>E</b> to interact with it.";
    }

    public void CombatTutorial()
    {
        tutorialText.text = "Now, let's learn about combat:  \n \n Use the left mouse button to attack \n \n Press <b>R</b> to reload \n \n ";
    }

    public void CompleteCombatTutorial()
    {
        tutorialText.text = "Excellent! Now follow the waypoint and complete your objectives";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            completedObjective?.Invoke();
        }
        
    }
}
