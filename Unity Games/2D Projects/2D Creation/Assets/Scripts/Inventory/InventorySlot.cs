using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* Represents a single slot in the inventory, containing an item and its quantity, along with references to related UI
 * elements and the inventory manager. Code inspired by and sourced by Night Run Studio https://www.youtube.com/playlist?list=PLSR2vNOypvs7sV_ks7h42F7hZ7DmGJqU6 */
public class InventorySlot : MonoBehaviour
{
    // Variables
    public ItemSO itemSO;
    public int quantity;
    public Image itemImage;
    public TMP_Text quantityText;

    // Update the UI for the Inventory slot in game 
    public void UpdateUI()
    {
        if(itemSO != null)
        {
            itemImage.sprite = itemSO.icon;
            itemImage.gameObject.SetActive(true);
            quantityText.text = quantity.ToString();
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            quantityText.text = "";
        }
    }
}
