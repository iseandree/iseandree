using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Counter is responsible for counting the number of times a player enters a trigger collider. It updates a UI TextMeshProUGUI element to display the count.
/// </summary>
public class Counter : MonoBehaviour
{
    public TextMeshProUGUI CounterText;
    private int Count = 0;

    private void Start()
    {
        CounterText.text = "Count : " + Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        Count += 1;
        CounterText.text = "Count : " + Count;
    }
}
