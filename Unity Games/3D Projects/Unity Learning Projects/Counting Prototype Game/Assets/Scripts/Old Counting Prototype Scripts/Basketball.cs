using UnityEngine;

public class Basketball : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f; // Lifetime of the basketball in seconds
    private float creationTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        creationTime += Time.deltaTime;
        // after five seconds delete the basketball
        if (lifetime <= creationTime)
        {
            Destroy(gameObject);
        }
    }
}
