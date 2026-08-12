using System.Collections;
using UnityEngine;

public class Lvl2VirtualObjectiveManager : MonoBehaviour
{
    private enum ObjectiveStage
    {
        DoorToOutside,
        Upstairs,
        Home,

        Complete
    }

    [Header("UI Systems")]
    [SerializeField] private TaskUIController taskUI;
    [SerializeField] private WaypointUI waypointUI;

    [Header("Waypoint Targets")]
    [SerializeField] private Transform doorTarget;
    [SerializeField] private Transform stairsTarget;
    [SerializeField] private Transform homeTarget;

    [Header("Interaction Objects")]
    [SerializeField] private GameObject doorInteraction;
    [SerializeField] private GameObject stairsInteraction;
    [SerializeField] private GameObject homeInteraction;

    [Header("Timing")]
    [SerializeField] private float nextTaskDelay = 1.5f;

    private ObjectiveStage currentStage;

    private void Start()
    {
        StartDoorToOutsideObjective();
    }

    private void StartDoorToOutsideObjective()
    {
        currentStage = ObjectiveStage.DoorToOutside;

        SetInteractionStates(
            doorActive: true,
            stairsActive: false,
            homeActive: false
        );

        taskUI.ShowTask("Go through the door to find out more");
        waypointUI.SetTarget(doorTarget);
        waypointUI.ShowWaypoint();
    }

    public void CompleteDoorToOutsideObjective()
    {
        if (currentStage != ObjectiveStage.DoorToOutside)
        {
            return;
        }

        doorInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();

        StartCoroutine(StartUpstairsAfterDelay());
    }

    private IEnumerator StartUpstairsAfterDelay()
    {
        yield return new WaitForSecondsRealtime(nextTaskDelay);

        currentStage = ObjectiveStage.Upstairs;

        SetInteractionStates(
            doorActive: false,
            stairsActive: true,
            homeActive: false
        );

        taskUI.ShowTask("Go upstairs to go home");
        waypointUI.SetTarget(stairsTarget);
        waypointUI.ShowWaypoint();
    }

    public void CompleteUpstairsObjective()
    {
        if (currentStage != ObjectiveStage.Upstairs)
        {
            return;
        }

        stairsInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();

        StartCoroutine(StartHomeAfterDelay());
    }

    private IEnumerator StartHomeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(nextTaskDelay);

        currentStage = ObjectiveStage.Home;

        SetInteractionStates(
            doorActive: false,
            stairsActive: false,
            homeActive: true
        );

        taskUI.ShowTask("Go back home and rest.");
        waypointUI.SetTarget(homeTarget);
        waypointUI.ShowWaypoint();
    }

    public void CompleteHomeObjective()
    {
        if (currentStage != ObjectiveStage.Home)
        {
            return;
        }

        currentStage = ObjectiveStage.Complete;

        homeInteraction.SetActive(false);
        waypointUI.HideWaypoint();
        taskUI.CompleteTask();
    }

    private void SetInteractionStates(
        bool doorActive,
        bool stairsActive,
        bool homeActive)
    {
        doorInteraction.SetActive(doorActive);
        stairsInteraction.SetActive(stairsActive);
        homeInteraction.SetActive(homeActive);

    }
}