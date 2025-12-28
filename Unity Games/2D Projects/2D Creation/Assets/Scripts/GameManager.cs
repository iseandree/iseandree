using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Temporary Setup
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI timerText;
    private SpriteRenderer backgroundColor;
    Color newColor;

    // Private Variables (Staying)
    private float countDown = 60.0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundColor = background.GetComponent<SpriteRenderer>();
        Debug.Log(backgroundColor);
        newColor = backgroundColor.color;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        timerText.text = countDown.ToString();

        if(countDown > 0)
        {
            newColor.b -= 0.002f;
            backgroundColor.color -= new Color(0,0, backgroundColor.color.b - newColor.b);
            
        }
        
    }
}
