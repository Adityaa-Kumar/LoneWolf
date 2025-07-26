using UnityEngine;
using UnityEngine.UI;

public class DetectBarUpdate : MonoBehaviour
{
    public GameManager GameManager;
    public Image DetectBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DetectBar.fillAmount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DetectBar.fillAmount = GameManager.detect;
    }
}
