using UnityEngine;

public class GameBallSpawner : MonoBehaviour
{
    // Serialize Field Variables
    [SerializeField] private GameObject ballPrefab;

    // Private variables
    private Vector3 startingPos = new Vector3(-2.65f, 13, -6.5f);

    // Spawn the Game Ball in the starting position
    public void SpawnGameBall()
    {
        Instantiate(ballPrefab, startingPos, transform.rotation);
    }
}
