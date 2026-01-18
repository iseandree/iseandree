using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    // private
    private int score = 1;
    private int lives = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            other.GetComponent<AnimalHunger>().FeedAnimal(score);
            Destroy(gameObject);
        }
        
        if(other.CompareTag("Player"))
        {
            GameManager.LoseLife(lives);
            Destroy(gameObject);

        }
    }
}
