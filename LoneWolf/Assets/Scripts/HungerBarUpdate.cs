using UnityEngine;
using UnityEngine.UI;

public class HungerBarUpdate : MonoBehaviour
{
    public GameManager GameManager;
    public Image HungerBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HungerBar.fillAmount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HungerBar.fillAmount = GameManager.hunger;
    }
}
