using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Private Variables
    [SerializeField] private GameObject enemyPrefab;
    private float spawnRange = 9.0f;
    private float spawnPosX;
    private float spawnPosZ;
    private Vector3 randomPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GenerateSpawnPosition()
    {
        spawnPosX = Random.Range(-spawnRange, spawnRange);
        spawnPosZ = Random.Range(-spawnRange, spawnRange);
        randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
}
