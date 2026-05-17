using System;
using UnityEngine;

public class Items : MonoBehaviour
{
    public ItemScriptableObject itemSO;
    public SpriteRenderer spriteRenderer;
    public int quantity;
    public static event Action<ItemScriptableObject, int> OnItemLooted;

    /// <summary>
    /// Performs validation logic when the component's properties are changed in the editor.
    /// </summary>
    /// <remarks>This method is called automatically by the Unity Editor when a value on the component is
    /// modified. Override this method to implement custom validation or to enforce constraints on serialized fields
    /// during editing.</remarks>
    private void OnValidate()
    {
        if(itemSO == null)
        {
            return;
        }
        spriteRenderer.sprite = itemSO.icon;
        this.name = itemSO.itemName;
    }

    public void Collect()
    {
        OnItemLooted?.Invoke(itemSO, quantity);
        Destroy(gameObject, .05f);
    }
}
