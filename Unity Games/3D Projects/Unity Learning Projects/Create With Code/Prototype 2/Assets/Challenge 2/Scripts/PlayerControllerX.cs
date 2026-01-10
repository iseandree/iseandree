using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    private float lastSpawnTime;
    [SerializeField] private float spawnDelay = 1.0f;

    private void Start()
    {
        lastSpawnTime = Time.time;    
    }

    // Update is called once per frame
    void Update()
    {
        lastSpawnTime += Time.deltaTime;
        // On spacebar press, send dog
        if (lastSpawnTime >= spawnDelay)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
                lastSpawnTime = 0;
            }
        }
    }
}
