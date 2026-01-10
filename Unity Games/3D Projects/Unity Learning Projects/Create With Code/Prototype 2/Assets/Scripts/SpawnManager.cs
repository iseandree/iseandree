using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Private Variables
    [SerializeField] private GameObject[] animalPrefabs;
    private float spawnRangeX = 18.0f;
    private float spawnRangeZ = 18.0f;
    private float startDelay = 2.0f;
    private float spawnInterval = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Spawn a random animal from the animalPrefabs array in random locations
    private void SpawnRandomAnimal()
    {
        int spawnRandomizer = Random.Range(0, 2);

        if(spawnRandomizer == 0)
        {
            SpawnHorizontally();
        }
        
        if(spawnRandomizer == 1)
        {
            SpawnVertically();
        }       
    }
    
    private void SpawnHorizontally()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnRangeZ);
        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        Instantiate(animalPrefabs[animalIndex], spawnPos, rotation);
    }

    private void SpawnVertically()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        int leftSideSpawn = -1;
        int rightSideSpawn = 1;
        int directionModifier = Random.Range(0, 2) * 2 - 1;

        if(directionModifier == leftSideSpawn)
        {
            Vector3 spawnPos = new Vector3(-spawnRangeX, 0, Random.Range(0, spawnRangeZ));
            Quaternion rotation = Quaternion.Euler(0, 90, 0);
            Instantiate(animalPrefabs[animalIndex], spawnPos, rotation);
        }
        
        if(directionModifier == rightSideSpawn)
        {
            Vector3 spawnPos = new Vector3(spawnRangeX, 0, Random.Range(0, spawnRangeZ));
            Quaternion rotation = Quaternion.Euler(0, 270, 0);
            Instantiate(animalPrefabs[animalIndex], spawnPos, rotation);
        }
    }
}
