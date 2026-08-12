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
        tutorialText.text = "This is the outer layer of my software \n \n Use <b>W A S D</b> to navigate the data";
    }

    public void MovementTutorial()
    {
            tutorialText.text = "Use <b>Space</b> to jump over clear gaps or jump over obstacles."; 
    }

    public void InteractionTutorial()
    {
        tutorialText.text = "Hold <b>Shift</b> while moving to dash and turn Immutable, this takes Energy but <b>Dashing through harmful data makes you take no damage</b>.";
    }

    public void CombatTutorial()
    {
        tutorialText.text = "Use the left mouse button to fire \n \n Press <b>R</b> to reload \n \n ";
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
