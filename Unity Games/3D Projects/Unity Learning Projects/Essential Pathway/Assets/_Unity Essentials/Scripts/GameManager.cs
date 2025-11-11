using UnityEngine;
using System; // Required for Type handling


public class GameManager : MonoBehaviour
{
    // Private variables
    private int totalCollectibles = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Check and count objects of type Collectible
        Type collectibleType = Type.GetType("Collectible");
        if (collectibleType != null)
        {
            totalCollectibles += UnityEngine.Object.FindObjectsByType(collectibleType, FindObjectsSortMode.None).Length;

        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Total collectibles = " + totalCollectibles);
        if (totalCollectibles == 0)
        {
            Application.Quit();
        }
    }

}
