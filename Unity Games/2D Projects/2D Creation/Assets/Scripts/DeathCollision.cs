using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            DataPersistenceManager.Instance.RestartGame();
            SceneManager.LoadScene("GameScene");
        }
    }
}
