using System.Collections;
using UnityEngine;

public class Lvl1VirtualObjectiveManager : MonoBehaviour
{
    private enum ObjectiveStage
    {
        DefeatEnemy,
        Boss,
        DefeatBoss,
        Home,

        Complete
    }

    [Header("UI Systems")]
    [SerializeField] private TaskUIController taskUI;

    [Header("Interaction Objects")]
    [SerializeField] private GameObject defeatEnemyInteraction;
    [SerializeField] private GameObject bossInteraction;
    [SerializeField] private GameObject defeatBossInteraction;
    [SerializeField] private GameObject homeInteraction;

    [Header("Timing")]
    [SerializeField] private float nextTaskDelay = 1.5f;

    private ObjectiveStage currentStage;

    private void Start()
    {
        StartDefeatEnemyObjective();
    }

    private void StartDefeatEnemyObjective()
    {
        currentStage = ObjectiveStage.DefeatEnemy;

        SetInteractionStates(
            defeatEnemyActive: true,
            bossActive: false,
            defeatBossActive: false,
            homeActive: false
        );

        taskUI.ShowTask("Defeat the enemies that is blocking your path");
    }

    public void CompleteDefeatEnemyObjective()
    {
        if (currentStage != ObjectiveStage.DefeatEnemy)
        {
            return;
        }

        defeatEnemyInteraction.SetActive(false);
        taskUI.CompleteTask();

        StartCoroutine(StartUpstairsAfterDelay());
    }

    private IEnumerator StartUpstairsAfterDelay()
    {
        yield return new WaitForSecondsRealtime(nextTaskDelay);

        currentStage = ObjectiveStage.Boss;

        SetInteractionStates(
            defeatEnemyActive: false,
            bossActive: true,
            defeatBossActive: false,
            homeActive: false
        );

        taskUI.ShowTask("Continue defeating the enemies and find the boss room");
    }

    public void CompleteBossObjective()
    {
        if (currentStage != ObjectiveStage.Boss)
        {
            return;
        }

        bossInteraction.SetActive(false);
        taskUI.CompleteTask();

        StartCoroutine(StartDefeatBossDelay());
    }

    private IEnumerator StartDefeatBossDelay()
    {
        yield return new WaitForSecondsRealtime(nextTaskDelay);

        currentStage = ObjectiveStage.DefeatBoss;

        SetInteractionStates(
            defeatEnemyActive: false,
            bossActive: false,
            defeatBossActive: true,
            homeActive: false
            
        );

        taskUI.ShowTask("Defeat the boss");
    }

    public void CompleteDefeatBossObjective()
    {
        if (currentStage != ObjectiveStage.DefeatBoss)
        {
            return;
        }

        currentStage = ObjectiveStage.Complete;

        defeatBossInteraction.SetActive(false);
        taskUI.CompleteTask();
    }

    private IEnumerator StartHomeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(nextTaskDelay);

        currentStage = ObjectiveStage.Home;

        SetInteractionStates(
            defeatEnemyActive: false,
            bossActive: false,
            defeatBossActive: false,
            homeActive: true
        );

        taskUI.ShowTask("Defeat the boss and go home");
    }

    public void CompleteHomeObjective()
    {
        if (currentStage != ObjectiveStage.Home)
        {
            return;
        }

        currentStage = ObjectiveStage.Complete;

        homeInteraction.SetActive(false);
        taskUI.CompleteTask();
    }

    private void SetInteractionStates(
        bool defeatEnemyActive,
        bool bossActive,
        bool defeatBossActive,
        bool homeActive)
    {
        defeatEnemyInteraction.SetActive(defeatEnemyActive);
        bossInteraction.SetActive(bossActive);
        defeatBossInteraction.SetActive(defeatBossActive);
        homeInteraction.SetActive(homeActive);

    }
}