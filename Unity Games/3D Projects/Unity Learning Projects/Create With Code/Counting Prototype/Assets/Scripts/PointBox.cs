using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PointBox : MonoBehaviour
{
    // Private Variables
    [SerializeField] private TextMeshProUGUI boxText;
    private int points;
    private int maxPoints = 50;
    private GameManager gameManager;
    private FallingBall fallingBall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        fallingBall = GameObject.Find("Game Ball").GetComponent<FallingBall>();
        points = UnityEngine.Random.Range(-maxPoints, maxPoints);
        boxText.text = points.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Game Ball"))
        {
            gameManager.AddPoints(points);
            Destroy(other.gameObject);
            ResetValues();
            fallingBall.ResetBall();
        }
    }

    // If the player ever decides to change the values provided in the boxes or after a ball is collected
    public void ResetValues()
    {
        points = UnityEngine.Random.Range(-maxPoints, maxPoints);
        boxText.text = points.ToString();
    }
}
