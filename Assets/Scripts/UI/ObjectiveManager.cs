using System.Collections;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private enum ObjectiveStage
    {
        RepairGenerator,
        TalkToZyr4,
        EatBreakfast,
        Complete
    }

    [Header("UI Systems")]
    [SerializeField] private TaskUIController taskUI;
    [SerializeField] private WaypointUI waypointUI;

    [Header("Waypoint Targets")]
    [SerializeField] private Transform generatorTarget;
    [SerializeField] private Transform zyr4Target;
    [SerializeField] private Transform foodTarget;
    [SerializeField] private Transform dailyTarget;

    [Header("Interaction Objects")]
    [SerializeField] private GameObject generatorInteraction;
    [SerializeField] private GameObject zyr4Interaction;
    [SerializeField] private GameObject foodInteraction;
    [SerializeField] private GameObject dailyInteraction;

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
            dailyTaskActive: false
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
            dailyTaskActive: false
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
            dailyTaskActive: false
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
    }

    private IEnumerator StartDailyTasksAfterDelay()
    {
     yield return new WaitForSecondsRealtime(nextTaskDelay);

     currentStage = ObjectiveStage.DailyTask;

        SetInteractionStates(
            generatorActive: false,
            zyr4Active: false,
            foodActive: false,
            dailyTaskActive: true
        );

        taskUI.ShowTask("Complete Daily Tasks: \n 1. Clean the dirt in the house \n 2. Clean the solar panels");
        waypointUI.SetTarget(dailyTarget);
        waypointUI.ShowWaypoint();  
    }

    public void CompleteDailyObjective()
    {
        if (currentStage != ObjectiveStage.DailyTask)
        {
            return;
        }

        currentStage = ObjectiveStage.Complete;

        dailyInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();
    } 

    private void SetInteractionStates(
        bool generatorActive,
        bool zyr4Active,
        bool foodActive,
        bool dailyTaskActive)
    {
        generatorInteraction.SetActive(generatorActive);
        zyr4Interaction.SetActive(zyr4Active);
        foodInteraction.SetActive(foodActive);
        dailyInteraction.SetActive(dailyTaskActive);
    }
}