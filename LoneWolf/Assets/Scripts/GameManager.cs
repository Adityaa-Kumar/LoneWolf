using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float hunger = 0f;
    [Range(0f, 1f)]
    public float detect = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}
