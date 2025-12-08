using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    [Header("Phase Panels")]
    [SerializeField] private GameObject playerPhase;
    [SerializeField] private GameObject attackPhase;

    [Header("Fight References")]
    [SerializeField] private BossHealth boss;
    [SerializeField] private HayBalePattern hayBalePattern;
    [SerializeField] private NeedlePattern needlePattern;

    private bool nextIsHayBales = true;
    private bool isRunningEnemyTurn = false;

    void Start()
    {
        ShowPlayerPhase();
    }

    public void ShowPlayerPhase()
    {
        playerPhase.SetActive(true);
        attackPhase.SetActive(false);
    }

    private void ShowAttackPhase()
    {
        playerPhase.SetActive(false);
        attackPhase.SetActive(true);
    }

    public void StartPlayerAttack(float damage)
    {
        if (boss == null || isRunningEnemyTurn) return;

        boss.AttackBoss(damage);

        if (boss.Health <= 0f)
        {
            EndFight();
            return;
        }

        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        isRunningEnemyTurn = true;
        ShowAttackPhase();

        IAttackPattern pattern = nextIsHayBales ? hayBalePattern : needlePattern;
        yield return pattern.Execute();

        nextIsHayBales = !nextIsHayBales;

        if (boss.Health > 0f)
            ShowPlayerPhase();
        else
            EndFight();

        isRunningEnemyTurn = false;
    }

    private void EndFight()
    {
        playerPhase.SetActive(false);
        attackPhase.SetActive(false);
        SceneManager.LoadScene("Selvain");
        Debug.Log("Boss defeated!");
    }
}

public interface IAttackPattern
{
    IEnumerator Execute();
}
