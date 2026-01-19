using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Private variables
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Vector3 spawnPos = new Vector3(25, 0, 0);
    [SerializeField] private float startDelay = 2.0f;
    [SerializeField] private float repeatRate = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Spawn Obstacle using Instantiate
    private void SpawnObstacle()
    {
        Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
    }


}
