using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    // Reference to your Game Over UI panel (assign in Inspector)
    public GameObject WinPanel;

    void Start()
    {
        // Hide the Game Over panel initially
        WinPanel.SetActive(false);
    }

    // Call this method when your game is over
    public void ShowGameOver()
    {
        WinPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    // Called by the Retry button OnClick event
    public void Retry()
    {
        Time.timeScale = 1f; // Unpause the game
        // Reload the currently active scene to restart the game
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Optional: Quit Game Method if you want a quit button
    public void QuitGame()
    {
        Application.Quit();
    }
}