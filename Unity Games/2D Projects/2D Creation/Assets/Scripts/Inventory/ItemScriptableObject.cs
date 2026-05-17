using UnityEngine;

[CreateAssetMenu(fileName = "New Item")]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite icon;
    public bool isFood;
    public bool isWater;
}
