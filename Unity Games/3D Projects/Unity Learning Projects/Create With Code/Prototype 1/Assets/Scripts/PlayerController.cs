using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float forwardInput;
    [SerializeField] private GameObject[] cameras;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        // We'll move the vehicle forward - using z axis
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);

        // We'll rotate the vehicle based on horizontal input
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        // If Camera Toggle input is pressed then switch camera
        if(cameras[0].activeSelf && Input.GetButtonDown("Camera Toggle"))
        {
            cameras[0].SetActive(false);
            cameras[1].SetActive(true);
            Console.WriteLine("Button Pressed");
        }
        else if (cameras[1].activeSelf && Input.GetButtonDown("Camera Toggle"))
        {
            cameras[0].SetActive(true);
            cameras[1].SetActive(false);
            Console.WriteLine("Button Pressed Again");
        }
        
    }
}
