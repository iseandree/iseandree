using UnityEngine;

/* Represents a configurable item definition for use in the game, including display information and item properties.
Code inspired by and sourced by Night Run Studio https://www.youtube.com/playlist?list=PLSR2vNOypvs7sV_ks7h42F7hZ7DmGJqU6 */
[CreateAssetMenu(fileName = "New Item")]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite icon;
    public int stackSize = 40;
}
