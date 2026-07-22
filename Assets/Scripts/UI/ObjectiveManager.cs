using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [Header("UI Systems")]
    [SerializeField] private TaskUIController taskUI;
    [SerializeField] private WaypointUI waypointUI;

    [Header("First Objective")]
    [SerializeField] private Transform zyr4WaypointTarget;
    [SerializeField] private string firstTaskText = "Talk to Zyr4";

    private bool firstObjectiveCompleted;

    private void Start()
    {
        StartTalkToZyr4Objective();
    }

    private void StartTalkToZyr4Objective()
    {
        firstObjectiveCompleted = false;

        if (taskUI != null)
        {
            taskUI.ShowTask(firstTaskText);
        }

        if (waypointUI != null)
        {
            waypointUI.SetTarget(zyr4WaypointTarget);
            waypointUI.ShowWaypoint();
        }
    }

    public void CompleteTalkToZyr4Objective()
    {
        if (firstObjectiveCompleted)
        {
            return;
        }

        firstObjectiveCompleted = true;

        if (waypointUI != null)
        {
            waypointUI.HideWaypoint();
        }

        if (taskUI != null)
        {
            taskUI.CompleteTask();
        }
    }
}