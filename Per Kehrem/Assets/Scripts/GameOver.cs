using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
