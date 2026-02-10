using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Private Variables
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] powerupPrefab;
    [SerializeField] private GameObject bossEnemyPrefab;
    //[SerializeField] private int enemiesToSpawn = 3;
    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int enemyCount;
    private bool waveInProgress = true;
    private float spawnRange = 9.0f;
    private float spawnPosX;
    private float spawnPosZ;
    private int randomEnemyInt;
    public int randomPowerupInt;
    private Vector3 randomPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomEnemyInt = Random.Range(0, 2);
        randomPowerupInt = Random.Range(0, 3);
        //SpawnEnemyWave(waveNumber);
        Instantiate(powerupPrefab[randomPowerupInt], GenerateSpawnPosition(), powerupPrefab[randomPowerupInt].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;

        if (enemyCount == 0 && waveInProgress)
        {
            waveInProgress = false;
            waveNumber++;

            if (waveNumber % 5 == 0)
            {
                SpawnBossBattle();
            }
            else
            {
                SpawnEnemyWave(waveNumber);
                Instantiate(
                    powerupPrefab[randomPowerupInt],
                    GenerateSpawnPosition(),
                    powerupPrefab[randomPowerupInt].transform.rotation
                );
            }
        }

        if (enemyCount > 0)
        {
            waveInProgress = true;
        }
    }



    // Method that randomly generates a spawning position for spawnable objects
    private Vector3 GenerateSpawnPosition()
    {
        spawnPosX = Random.Range(-spawnRange, spawnRange);
        spawnPosZ = Random.Range(-spawnRange, spawnRange);
        randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    // Method that spawns a new wave of enemies with a value passed to it
    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab[randomEnemyInt], GenerateSpawnPosition(), enemyPrefab[randomEnemyInt].transform.rotation);
        }
    }


    private void SpawnBossBattle()
    {
        Instantiate(bossEnemyPrefab, GenerateSpawnPosition(), bossEnemyPrefab.transform.rotation);
        SpawnEnemyWave(3);
    }
}
