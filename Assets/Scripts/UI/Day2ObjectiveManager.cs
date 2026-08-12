using System.Collections;
using UnityEngine;

public class Day2ObjectiveManager : MonoBehaviour
{
    private enum ObjectiveStage
    {        
        TalkToZyr4,
        DirtTask,
        WaterTask,
        Zyr4BugTask,
        TrainDoorTask,
        Complete
    }

    [Header("UI Systems")]
    [SerializeField] private TaskUIController taskUI;
    [SerializeField] private WaypointUI waypointUI;

    [Header("Waypoint Targets")]
    [SerializeField] private Transform zyr4Target;
    [SerializeField] private Transform dirtTarget;
    [SerializeField] private Transform waterTarget;
    [SerializeField] private Transform zyr4BugTarget;
    [SerializeField] private Transform trainDoorTarget;

    [Header("Interaction Objects")]
    [SerializeField] private GameObject zyr4Interaction;
    [SerializeField] private GameObject dirt;
    [SerializeField] public GameObject waterInteraction;
    [SerializeField] public GameObject zyr4BugInteraction;
    [SerializeField] public GameObject trainDoorInteraction;

    [SerializeField] private int dirtCleaned = 0;

    [Header("Timing")]
    [SerializeField] private float nextTaskDelay = 1.5f;

    private ObjectiveStage currentStage;

    private void Start()
    {
        StartZyr4Objective();
    }
    private void StartZyr4Objective()
    {
        currentStage = ObjectiveStage.TalkToZyr4;

        SetInteractionStates(
            zyr4Active: true,
            dirtTaskActive: false,
            waterActive: false,
            zyr4BugActive: false,
            TrainDoorActive: false
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

        StartCoroutine(StartDirtTasksAfterDelay());
    }

    private IEnumerator StartDirtTasksAfterDelay()
    {
     yield return new WaitForSecondsRealtime(nextTaskDelay);

     currentStage = ObjectiveStage.DirtTask;

        SetInteractionStates(
            zyr4Active: false,
            dirtTaskActive: true,
            waterActive: false,
            zyr4BugActive: false,
            TrainDoorActive: false
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
            zyr4Active: false,
            dirtTaskActive: false,
            waterActive: true,
            zyr4BugActive: false,
            TrainDoorActive: false
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
            zyr4Active: false,
            dirtTaskActive: false,
            waterActive: false,
            zyr4BugActive: true,
            TrainDoorActive: false
        );

        zyr4BugInteraction.SetActive(true);
        taskUI.ShowTask("Head back into the house and talk to Zyr4");
        waypointUI.SetTarget(zyr4Target);
        waypointUI.ShowWaypoint();   
    }

    public void CompleteZyr4BugObjective()
    {
        if (currentStage != ObjectiveStage.Zyr4BugTask)
        {
            return;
        }
        
        currentStage = ObjectiveStage.Complete;

        zyr4BugInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();

        StartCoroutine(StartHeadToTrainDoorAfterDelay());
    } 

    public IEnumerator StartHeadToTrainDoorAfterDelay()
    {
        yield return new WaitForSecondsRealtime(nextTaskDelay);

        currentStage = ObjectiveStage.TrainDoorTask;

        SetInteractionStates(
            zyr4Active: false,
            dirtTaskActive: false,
            waterActive: false,
            zyr4BugActive: false,
            TrainDoorActive: true
        );

        trainDoorInteraction.SetActive(true);
        taskUI.ShowTask("Find the train door to go deeper into Zyr4's head");
        waypointUI.SetTarget(trainDoorTarget);
        waypointUI.ShowWaypoint();  
    }

    public void CompleteTrainDoorObjective()
    {
        if (currentStage != ObjectiveStage.TrainDoorTask)
        {
            return;
        }
        
        currentStage = ObjectiveStage.Complete;

        trainDoorInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();
    } 

    private void SetInteractionStates(
        bool zyr4Active,
        bool dirtTaskActive,
        bool waterActive,
        bool zyr4BugActive,
        bool TrainDoorActive)
    {
        zyr4Interaction.SetActive(zyr4Active);
        dirt.SetActive(dirtTaskActive);
        waterInteraction.SetActive(waterActive);
        zyr4BugInteraction.SetActive(zyr4BugActive);
        trainDoorInteraction.SetActive(TrainDoorActive);

    }
}