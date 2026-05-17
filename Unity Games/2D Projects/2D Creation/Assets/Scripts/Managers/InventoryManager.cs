using System.Data;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    public InventorySlot[] itemSlots;
    public int food;
    public TMP_Text foodText;
    public int water;
    public TMP_Text waterText;


    private void OnEnable()
    {
        Items.OnItemLooted += AddItem;
    }
    private void OnDisable()
    {
        Items.OnItemLooted -= AddItem;
    }

    private void Start()
    {
        foreach (var slot in itemSlots)
        {
            slot.UpdateUI();
        }
    }

    private void AddItem(ItemScriptableObject itemSO, int quantity)
    {
        if (itemSO.isFood)
        {
            food += quantity;
            foodText.text = quantity.ToString();
            return;
        }
        else if (itemSO.isWater)
        {
            water += quantity;
            waterText.text = quantity.ToString();
            return;
        }
        else
        {
            foreach (var slot in itemSlots)
            {
                if (slot.itemSO == null)
                {
                    slot.itemSO = itemSO;
                    slot.quantity = quantity;
                    slot.UpdateUI();
                    return;
                }
            }
        }
    }

    public void UseItem(InventorySlot slot)
    {
        if(slot.itemSO != null && slot.quantity >= 0)
        {
            Debug.Log("Trying to use item: " + slot.itemSO.itemName);
        }
        else
        {
            Debug.Log("No item available");
        }
    }

    public void SaveData(ref GameData data)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            var slot = itemSlots[i];
            if (slot.itemSO != null)
            {
                data.slot.itemSO = itemSlots[i].itemSO;
                data.slot.quantity = itemSlots[i].quantity;
                return;
            }
            Debug.Log("Slot: " + slot +
            " Slot Item SO: " + slot.itemSO + " Slot quantity: " + slot.quantity);
        }
    }

    public void LoadData(GameData data)
    {
        if (data == null || data.slot == null) return;

        for (int i = 0; i < itemSlots.Length; i++)
        {
            var slot = itemSlots[i];
            if (slot.itemSO != null)
            {
                // Copy saved values into the existing slot instance instead of assigning the foreach iteration variable.
                itemSlots[i].itemSO = data.slot.itemSO;
                itemSlots[i].quantity = data.slot.quantity;
                itemSlots[i].UpdateUI();
                return;
            }
            Debug.Log("Slot: " + slot +
            " Slot Item SO: " + slot.itemSO + " Slot quantity: " + slot.quantity);
        }
        
    }
}
