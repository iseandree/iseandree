using System;
using System.Data;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

/* Manages the player's inventory, including item slots, consumable resources, and item usage within the game.
 Code inspired by and sourced by Night Run Studio https://www.youtube.com/playlist?list=PLSR2vNOypvs7sV_ks7h42F7hZ7DmGJqU6 */
public class InventoryManager : MonoBehaviour, IDataPersistence
{
    public static InventoryManager Instance;

    // Variables - Item related
    public InventorySlot[] itemSlots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Subscribes to the item looted event when the component is enabled.
    private void OnEnable()
    {
        Items.OnItemLooted += AddItem;
    }

    // Handles cleanup when the component is disabled by unsubscribing from the item looted event.
    private void OnDisable()
    {
        Items.OnItemLooted -= AddItem;
    }

    // Initializes the item slots and updates their user interface to reflect the current state.
    private void Start()
    {
        foreach (var slot in itemSlots)
        {
            slot.UpdateUI();
        }
    }

    // Adds the specified quantity of an item to the inventory, stacking with existing items when possible or placing
    // in an empty slot.
    private void AddItem(ItemScriptableObject itemSO, int quantity)
    {
        // Stack if it is the same item and there is room to do so
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == itemSO && slot.quantity < itemSO.stackSize)
            {
                int availableSpace = itemSO.stackSize - slot.quantity;
                int amountToAdd = Mathf.Min(availableSpace, quantity);
                slot.quantity += amountToAdd;
                quantity -= amountToAdd;
                slot.UpdateUI();
                if (quantity <= 0)
                {
                    return;
                }
            }
        }

        // If there are more items look for the next empty slot to fill
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == null)
            {
                int amountToAdd = Mathf.Min(itemSO.stackSize, quantity);
                slot.itemSO = itemSO;
                slot.quantity = quantity;
                slot.UpdateUI();
                return;
            }
        }

        // If quantity is still greater than 0 then inventory is full
        if (quantity > 0)
        {
            DropItem(itemSO, quantity);
        }
    }

    // Removes the specified item from the inventory and just deletes it from the game world.
    private void DropItem(ItemScriptableObject itemSO, int quantity)
    {
        Debug.Log("inventory is full");
    }

    // Attempts to use the item in the specified inventory slot, decrementing its quantity and updating the slot state.
    public void UseItem(InventorySlot slot)
    {
        if (HasItem(slot.itemSO))
        {
            // If the slot item is not empty and there are more than 0 then give them to the NPC
            if (slot.itemSO != null && slot.quantity >= 0)
            {
                Debug.Log("Trying to use item: " + slot.itemSO.itemName);
                slot.quantity--;

                if (slot.quantity <= 0)
                {
                    slot.itemSO = null;
                }
                slot.UpdateUI();
            }
            else
            {
                Debug.Log("No item available");
            }
        }
    }

    public bool HasItem(ItemScriptableObject itemSO)
    {
        Debug.Log($"[Inventory Check] NPC is asking for: {itemSO.itemName}.");
        // If the system is looking for a food item, check the raw integer tracker

        // Otherwise, check the regular physical inventory slots
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == itemSO && slot.quantity > 0)
            {
                return true;
            }
        }
        return false;
    }

    // Saves the current item slot data into the specified structure.
    public void SaveData(ref GameData data)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            var slot = itemSlots[i];
            if (slot.itemSO != null)
            {
                data.slot.itemSO = itemSlots[i].itemSO;
                data.slot.quantity = itemSlots[i].quantity;
            }
            Debug.Log("Slot: " + slot +
            " Slot Item SO: " + slot.itemSO + " Slot quantity: " + slot.quantity);
        }
    }

    // Loads item slot data from the specified game data object into the current instance.
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
            }
            Debug.Log("Slot: " + slot +
            " Slot Item SO: " + slot.itemSO + " Slot quantity: " + slot.quantity);
        }
    }
}
