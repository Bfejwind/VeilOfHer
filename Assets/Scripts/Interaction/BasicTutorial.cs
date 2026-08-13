using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class BasicTutorial : MonoBehaviour
{
    [Header("Tutorial Text")]
    [SerializeField]
    public TMP_Text tutorialText;

    [Header("Tutorial Events")]
    [SerializeField] private UnityEvent completedObjective;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialText.gameObject.SetActive(true);
        StartTutorial();
    }

    public void StartTutorial()
    {
        tutorialText.text = "Hey! Looks like youre new here, let's start with a tutorial.\n \n Use <b>W A S D</b> to move around";
    }

    public void MovementTutorial()
    {
            tutorialText.text = "Awesome!\n\nNow you can hold <b>Shift</b> to run faster and <b>Space</b> to jump.\n\nTry heading towards the waypoint!"; 
    }

    public void InteractionTutorial()
    {
        tutorialText.text = "You can follow the objectives on the left!\n\nNow try pressing <b>V</b> to interact with the light.";
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
