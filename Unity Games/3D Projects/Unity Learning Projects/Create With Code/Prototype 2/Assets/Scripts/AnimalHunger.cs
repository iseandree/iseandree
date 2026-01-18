using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class AnimalHunger : MonoBehaviour
{
    // Private variables
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private int amountToBeFed;
    private int currentFedAmount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hungerSlider.maxValue = amountToBeFed;
        hungerSlider.value = 0;
        hungerSlider.fillRect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FeedAnimal(int amount)
    {
        currentFedAmount += amount;
        hungerSlider.fillRect.gameObject.SetActive(true);
        hungerSlider.value = currentFedAmount;

        if(currentFedAmount >= amountToBeFed)
        {
            GameManager.AddScore(amountToBeFed);
            Destroy(gameObject, 0.1f);
        }
    }
}
