using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject itemSO;
    public int quantity;

    public Image itemImage;
    public TMP_Text quantityText;

    public InventoryManager inventoryManager;

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
