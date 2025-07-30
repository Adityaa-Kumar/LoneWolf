using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Reference to your Game Over UI panel (assign in Inspector)
    public GameObject pausePanel;

    void Start()
    {
        // Hide the Game Over panel initially
        pausePanel.SetActive(false);
    }

    // Call this method when your game is over
    public void ShowPauseScreen()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    // Called by the Retry button OnClick event
    public void Resume()
    {
        Time.timeScale = 1f; // Unpause the game
        pausePanel.SetActive(false);
    }

    public void MM()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Optional: Quit Game Method if you want a quit button
    public void QuitGame()
    {
        Application.Quit();
    }
}