using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PointBox : MonoBehaviour
{
    // Serialize Field Variables
    [SerializeField] private TextMeshProUGUI boxText;
    [SerializeField] private ParticleSystem goodParticles;
    [SerializeField] private ParticleSystem badParticles;

    // Private Variables
    private int points;
    private int maxPoints = 50;
    private bool isNegative = false;
    private GameManager gameManager;
    private FallingBall fallingBall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize variables
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        fallingBall = GameObject.Find("Game Ball").GetComponent<FallingBall>();
        points = UnityEngine.Random.Range(-maxPoints, maxPoints);
        boxText.text = points.ToString();
        
        // Check if the number is positive or negative
        if (points < 0)
        {
            isNegative = true;
        }
        else if(points >= 0)
        {
            isNegative = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // On Trigger Enter is called when the game object's collider trigger is activated
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Game Ball"))
        {
            if (isNegative)
            {
                badParticles.Play();
            }
            else if (!isNegative)
            {
                goodParticles.Play();
            }

            gameManager.AddPoints(points);
            Destroy(other.gameObject);
            //ResetValues();
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
