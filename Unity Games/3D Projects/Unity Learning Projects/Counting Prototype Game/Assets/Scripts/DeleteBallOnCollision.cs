using UnityEngine;

public class DeleteBallOnCollision : MonoBehaviour
{
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        // Destroy the game object involved in the collision
        if (collision.gameObject.CompareTag("Basketball"))
        {
            Destroy(collision.gameObject);
        }
    }
}
