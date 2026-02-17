using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]

public class ClickAndSwipe : MonoBehaviour
{
	//Private Variables
	private GameManager gameManager;
	private Camera mainCamera;
	private Vector3 mousePos;
	private TrailRenderer trail;
	private BoxCollider boxCollider;
	private bool isSwiping = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        trail = GetComponent<TrailRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        trail.enabled = false;
        boxCollider.enabled = false;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isGameActive)
        {
            if(Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                UpdateComponents();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isSwiping = false;
                UpdateComponents();
            }

            if(isSwiping)
            {
                UpdateMousePosition();
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Target>())
        {
            collision.gameObject.GetComponent<Target>().DestroyObject();
        }
    }


    private void UpdateMousePosition()
    {
        mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }

    private void UpdateComponents()
    {
        trail.enabled = isSwiping;
        boxCollider.enabled = isSwiping;
    }

}
