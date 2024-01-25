using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public playerController player;
    public ScoreManager score;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        player.ChangeGameState(playerController.GameState.Play);
        score.StartTimer();
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        player.ChangeGameState(playerController.GameState.Pause);
        score.stopTimer();
        GameIsPaused = true;
    }

    public void Reset()
    {
        Debug.Log("Resetting game...");
        player.ChangeGameState(playerController.GameState.Play);
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
