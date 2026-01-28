using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;
    private Vector3 offset;

    private void Awake()
    {
        offset = transform.position - playerTarget.position;
    }

    private void LateUpdate()
    {
        transform.position = playerTarget.position + offset;
    }
}
