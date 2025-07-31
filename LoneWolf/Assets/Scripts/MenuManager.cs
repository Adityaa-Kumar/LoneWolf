using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Main Scene");
    }

    // Optional: Quit Game Method if you want a quit button
    public void QuitGame()
    {
        Application.Quit();
    }
}