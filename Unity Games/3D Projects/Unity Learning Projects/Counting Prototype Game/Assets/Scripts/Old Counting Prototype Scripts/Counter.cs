using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public TextMeshProUGUI CounterText;
    private int Count = 0;

    private void Start()
    {
        Count = 0;
        CounterText.text = "Count : " + Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        Count += 1;
        CounterText.text = "Count : " + Count;
    }
}
