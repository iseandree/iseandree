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

    public void SaveData(ref GameData data)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO != null)
            {
                data.slot.itemSO = slot.itemSO;
                data.slot.quantity = slot.quantity;
                return;
            }
        }
    }

    public void LoadData(GameData data)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO != null)
            {
                slot.itemSO = data.slot.itemSO;
                slot.quantity = data.slot.quantity;
                return;

            }
            Debug.Log("Slot: " + slot +
            " Slot Item SO: " + slot.itemSO + " Slot quantity: " + slot.quantity);
        }
        
    }
}
