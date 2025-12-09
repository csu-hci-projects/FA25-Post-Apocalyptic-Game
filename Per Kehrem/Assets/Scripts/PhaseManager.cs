using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    [Header("Phase Panels")]
    [SerializeField] private GameObject playerPhase;
    [SerializeField] private GameObject attackPhase;
    [SerializeField] private GameObject gameOverPhase;

    [Header("Fight References")]
    [SerializeField] private BossHealth boss;
    [SerializeField] private HayBalePattern hayBalePattern;
    [SerializeField] private NeedlePattern needlePattern;

    private bool nextIsHayBales = true;
    private bool isRunningEnemyTurn = false;
    private bool isGameOver = false;

    void Start()
    {
        ShowPlayerPhase();
        gameOverPhase.SetActive(false);
    }

    public void ShowPlayerPhase()
    {
        if (isGameOver) return;
        playerPhase.SetActive(true);
        attackPhase.SetActive(false);
        gameOverPhase.SetActive(false);
    }

    private void ShowAttackPhase()
    {
        if (isGameOver) return;
        playerPhase.SetActive(false);
        attackPhase.SetActive(true);
        gameOverPhase.SetActive(false);
    }

    public void StartPlayerAttack(float damage)
    {
        if (boss == null || isRunningEnemyTurn || isGameOver) return;

        boss.AttackBoss(damage);

        if (boss.Health <= 0f)
        {
            EndBossFight();
            return;
        }

        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        if (isGameOver) yield break;

        isRunningEnemyTurn = true;
        ShowAttackPhase();

        IAttackPattern pattern = nextIsHayBales ? hayBalePattern : needlePattern;
        yield return pattern.Execute();

        nextIsHayBales = !nextIsHayBales;

        if (!isGameOver)
        {
            if (boss.Health > 0f)
                ShowPlayerPhase();
            else
                EndBossFight();
        }

        isRunningEnemyTurn = false;
    }

    private void EndBossFight()
    {
        isGameOver = true;
        playerPhase.SetActive(false);
        attackPhase.SetActive(false);
        gameOverPhase.SetActive(false);
        SceneManager.LoadScene("Selvain");
    }

    public void EndPlayerDefeat()
    {
        isGameOver = true;
        playerPhase.SetActive(false);
        attackPhase.SetActive(false);
        gameOverPhase.SetActive(true);
    }

    public void LoadAgain()
    {
        SceneManager.LoadScene("Tutorial");
    }
}

public interface IAttackPattern
{
    IEnumerator Execute();
}
