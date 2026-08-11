using System.Collections;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private enum ObjectiveStage
    {
        RepairGenerator,
        TalkToZyr4,
        EatBreakfast,
        DirtTask,
        WaterTask,
        Zyr4BugTask,

        Complete
    }

    [Header("UI Systems")]
    [SerializeField] private TaskUIController taskUI;
    [SerializeField] private WaypointUI waypointUI;

    [Header("Waypoint Targets")]
    [SerializeField] private Transform generatorTarget;
    [SerializeField] private Transform zyr4Target;
    [SerializeField] private Transform foodTarget;
    [SerializeField] private Transform dirtTarget;
    [SerializeField] private Transform waterTarget;
    [SerializeField] private Transform zyr4BugTarget;

    [Header("Interaction Objects")]
    [SerializeField] private GameObject generatorInteraction;
    [SerializeField] private GameObject zyr4Interaction;
    [SerializeField] private GameObject foodInteraction;
    [SerializeField] private GameObject dirt;
    [SerializeField] public GameObject waterInteraction;
    [SerializeField] public GameObject zyr4BugInteraction;
    [SerializeField] public int dirtCleaned = 0; 

    [Header("Timing")]
    [SerializeField] private float nextTaskDelay = 1.5f;

    private ObjectiveStage currentStage;

    private void Start()
    {
        StartGeneratorObjective();
    }

    private void StartGeneratorObjective()
    {
        currentStage = ObjectiveStage.RepairGenerator;

        SetInteractionStates(
            generatorActive: true,
            zyr4Active: false,
            foodActive: false,
            dirtTaskActive: false,
            waterActive: false,
            zyr4BugActive: false
        );

        taskUI.ShowTask("Repair the power unit");
        waypointUI.SetTarget(generatorTarget);
        waypointUI.ShowWaypoint();
    }

    public void CompleteGeneratorObjective()
    {
        if (currentStage != ObjectiveStage.RepairGenerator)
        {
            return;
        }

        generatorInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();

        StartCoroutine(StartZyr4AfterDelay());
    }

    private IEnumerator StartZyr4AfterDelay()
    {
        yield return new WaitForSecondsRealtime(nextTaskDelay);

        currentStage = ObjectiveStage.TalkToZyr4;

        SetInteractionStates(
            generatorActive: false,
            zyr4Active: true,
            foodActive: false,
            dirtTaskActive: false,
            waterActive: false,
            zyr4BugActive: false
        );

        taskUI.ShowTask("Return inside and talk to Zyr4");
        waypointUI.SetTarget(zyr4Target);
        waypointUI.ShowWaypoint();
    }

    public void CompleteZyr4Objective()
    {
        if (currentStage != ObjectiveStage.TalkToZyr4)
        {
            return;
        }

        zyr4Interaction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();

        StartCoroutine(StartBreakfastAfterDelay());
    }

    private IEnumerator StartBreakfastAfterDelay()
    {
        yield return new WaitForSecondsRealtime(nextTaskDelay);

        currentStage = ObjectiveStage.EatBreakfast;

        SetInteractionStates(
            generatorActive: false,
            zyr4Active: false,
            foodActive: true,
            dirtTaskActive: false,
            waterActive: false,
            zyr4BugActive: false
        );

        taskUI.ShowTask("Eat breakfast");
        waypointUI.SetTarget(foodTarget);
        waypointUI.ShowWaypoint();
    }

    public void CompleteBreakfastObjective()
    {
        if (currentStage != ObjectiveStage.EatBreakfast)
        {
            return;
        }

        currentStage = ObjectiveStage.Complete;

        foodInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();

        StartCoroutine(StartDirtTasksAfterDelay());
    }

    private IEnumerator StartDirtTasksAfterDelay()
    {
     yield return new WaitForSecondsRealtime(nextTaskDelay);

     currentStage = ObjectiveStage.DirtTask;

        SetInteractionStates(
            generatorActive: false,
            zyr4Active: false,
            foodActive: false,
            dirtTaskActive: true,
            waterActive: false,
            zyr4BugActive: false
        );

        taskUI.ShowTask("Complete Daily Tasks: \n \n 1. Clean the solar panels " + dirtCleaned.ToString() + "/6  \n \n 2. Water the plants 0/1");
        waypointUI.SetTarget(dirtTarget);
        waypointUI.ShowWaypoint();  
    }

    public void CompleteDirtObjective()
    {
        if (currentStage != ObjectiveStage.DirtTask)
        {
            return;
        }
        currentStage = ObjectiveStage.Complete;

        waypointUI.HideWaypoint();
        taskUI.CompleteTask(); 

        StartCoroutine(StartWaterTaskAfterDelay());
    } 

    private IEnumerator StartWaterTaskAfterDelay()
    {
       yield return new WaitForSecondsRealtime(nextTaskDelay);

     currentStage = ObjectiveStage.WaterTask;

        SetInteractionStates(
            generatorActive: false,
            zyr4Active: false,
            foodActive: false,
            dirtTaskActive: false,
            waterActive: true,
            zyr4BugActive: false
        );

        waterInteraction.SetActive(true);
        taskUI.ShowTask("Complete Daily Tasks: \n \n <s>1. Clean the solar panels 6/6</s> \n \n 2. Water the plants 0/1");
        waypointUI.SetTarget(waterTarget);
        waypointUI.ShowWaypoint();   
    }

    public void CompleteWaterObjective()
    {
        if (currentStage != ObjectiveStage.WaterTask)
        {
            return;
        }
        
        currentStage = ObjectiveStage.Complete;

        waterInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();

        StartCoroutine(StartZyr4BugTaskAfterDelay());
    } 

    private IEnumerator StartZyr4BugTaskAfterDelay()
    {
       yield return new WaitForSecondsRealtime(nextTaskDelay);

     currentStage = ObjectiveStage.Zyr4BugTask;

        SetInteractionStates(
            generatorActive: false,
            zyr4Active: false,
            foodActive: false,
            dirtTaskActive: false,
            waterActive: true,
            zyr4BugActive: true
        );

        waterInteraction.SetActive(true);
        taskUI.ShowTask("Complete Daily Tasks: \n \n 1. Clean the solar panels 6/6 \n \n 2. Water the plants 0/1");
        waypointUI.SetTarget(waterTarget);
        waypointUI.ShowWaypoint();   
    }

    public void CompleteZyr4BugObjective()
    {
        if (currentStage != ObjectiveStage.Zyr4BugTask)
        {
            return;
        }
        
        currentStage = ObjectiveStage.Complete;

        waterInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();

        // StartCoroutine(StartDefeatEnemyTasksAfterDelay());
    } 

    private void SetInteractionStates(
        bool generatorActive,
        bool zyr4Active,
        bool foodActive,
        bool dirtTaskActive,
        bool waterActive,
        bool zyr4BugActive)
    {
        generatorInteraction.SetActive(generatorActive);
        zyr4Interaction.SetActive(zyr4Active);
        foodInteraction.SetActive(foodActive);
        dirt.SetActive(dirtTaskActive);
        waterInteraction.SetActive(waterActive);
        zyr4BugInteraction.SetActive(zyr4BugActive);
    }
}