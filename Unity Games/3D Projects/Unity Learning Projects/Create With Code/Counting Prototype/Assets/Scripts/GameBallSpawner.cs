using UnityEngine;

public class GameBallSpawner : MonoBehaviour
{
    // Private variables
    [SerializeField] private GameObject ballPrefab;
    private Vector3 startingPos = new Vector3(-2.65f, 13, -6.5f);

    public void SpawnGameBall()
    {
        Instantiate(ballPrefab, startingPos, transform.rotation);
    }
}
