using System;
using UnityEngine;

/* Represents a collectible item in the game world, supporting persistence of its collected state and integration with
 Unity's component system. Code inspired by and sourced by Night Run Studio https://www.youtube.com/playlist?list=PLSR2vNOypvs7sV_ks7h42F7hZ7DmGJqU6 */
public class Items : MonoBehaviour, IDataPersistence
{
    // Generate a random id for items so we can keep track of which have been collected
    [SerializeField] private string iD;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        iD = System.Guid.NewGuid().ToString();
    }

    // Variables - What is an item
    public ItemSO itemSO;
    public SpriteRenderer spriteRenderer;
    public int quantity;
    public bool isCollected;
    public static event Action<ItemSO, int> OnItemLooted;

    // Performs validation logic when the component's properties are changed in the editor.
    private void OnValidate()
    {
        if(itemSO == null)
        {
            return;
        }
        spriteRenderer.sprite = itemSO.icon;
        this.name = itemSO.itemName;
    }

    // Marks the item as collected and triggers the item looted event.
    public void Collect()
    {
        OnItemLooted?.Invoke(itemSO, quantity);
        isCollected = true;
        Destroy(gameObject, .05f);
    }

    // Updates the specified <see cref="GameData"/> instance to reflect the current collection state of this item.
    public void SaveData(ref GameData data)
    {
        if(data.itemsCollected.ContainsKey(iD))
        {
            data.itemsCollected.Remove(iD);
        }

        data.itemsCollected.Add(iD, isCollected);
    }

    // Loads the collected item state from the specified game data and updates the object's active status accordingly.
    public void LoadData(GameData data)
    {
        data.itemsCollected.TryGetValue(iD, out isCollected);
        if(isCollected)
        {
            gameObject.SetActive(false);
        }
    }
}
