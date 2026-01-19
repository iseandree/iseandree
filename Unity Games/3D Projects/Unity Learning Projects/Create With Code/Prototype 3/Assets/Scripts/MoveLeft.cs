using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Private variables
    [SerializeField] private float speed = 30.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
