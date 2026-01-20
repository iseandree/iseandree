using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    // Private variables
    [SerializeField] private Vector3 startPos;
    [SerializeField] private float resetDist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        resetDist = GetComponent<BoxCollider>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < startPos.x - resetDist)
        {
            transform.position = startPos;
        }
    }
}
