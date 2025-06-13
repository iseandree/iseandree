using UnityEngine;
using TMPro;

public class BallSpawnManager : MonoBehaviour
{
    public GameObject BasketballGameObject;
    int numberOfBalls = 0;
    public TextMeshProUGUI BallCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If the number of balls is 0, instantiate a basketball game object
        if (GameObject.FindGameObjectsWithTag("Basketball").Length == 0)
        {
            Instantiate(BasketballGameObject, transform.position, Quaternion.identity);
            numberOfBalls = GameObject.FindGameObjectsWithTag("Basketball").Length;
            BallCount.text = "# of Balls: " + numberOfBalls;
        }
    }
}
