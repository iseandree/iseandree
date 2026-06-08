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

    // Public Variables - Item related
    public InventorySlot[] itemSlots;
    [SerializeField] private ItemSO[] itemDatabase;
    public float aura;
    public static event Action<float> OnAuraIncreased;

    // Using the singleton design pattern Instance must be set and initialized immediately
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
    public void AddItem(ItemSO itemSO, int quantity)
    {
        // If the item is aura then just set the amount of aura based on the quantity and exit
        if(itemSO.isAura)
        {
            aura = quantity;
            return;
        }

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
    // This should not be necesarry as the amount of any particular item should not exceed whats needed.
    private void DropItem(ItemSO itemSO, int quantity)
    {
        Debug.Log("inventory is full");
    }

    // Remove the necessary item from the player's inventory in response to completing requirements for an objective
    public void RemoveItem(ItemSO itemSO, int quantity)
    {
        for(int i = 0; i< itemSlots.Length; i++)
        {
            var slot = itemSlots[i];
            if(slot.itemSO != itemSO)
            {
                continue;
            }

            if (slot.quantity > quantity)
            {
                slot.quantity -= quantity;
                slot.UpdateUI();
                quantity = 0;
            }
            else
            {
                quantity -= slot.quantity;
                slot.itemSO = null;
                slot.quantity = 0;
                slot.UpdateUI();
            }
        }
    }

    // Checks if the player has a particular item in their inventory
    public bool HasItem(ItemSO itemSO)
    {
        // Check every slot and see if that slot has the referenced item
        foreach (var slot in itemSlots)
        {
            // If they match and there is more than zero of that item return true
            if (slot.itemSO == itemSO && slot.quantity > 0)
            {
                return true;
            }
        }

        return false;
    }

    // Get the amount of a particular item and return the value for use elsewhere
    public int GetItemQuantity(ItemSO itemSO)
    {
        int total = 0;

        foreach(var slot in itemSlots)
        {
            if(slot.itemSO == itemSO)
            {
                total += slot.quantity;
            }
        }
        return total;
    }

    // Get the itemSo by its itemID and return it
    private ItemSO GetItemByID(string itemID)
    {
        foreach (ItemSO item in itemDatabase)
        {
            if (item.itemID == itemID)
            {
                return item;
            }
        }

        return null;
    }

    // Invokes the necessary event to increase the scale of the aura of the player properly passing the amount to increase by
    public void ChangeAuraScale(float amount)
    {
        OnAuraIncreased?.Invoke(amount);
    }

    // Saves the current item slot data into the specified structure.
    public void SaveData(ref GameData data)
    {
        data.inventorySlots.Clear();
        
        foreach(var slot in itemSlots)
        {
            if (slot.itemSO == null)
            { 
                continue;
            }

            InventorySlotData slotData = new InventorySlotData
            {
                itemID = slot.itemSO.itemID,
                quantity = slot.quantity
            };

            data.inventorySlots.Add(slotData);
        }
    }

    // Loads item slot data from the specified game data object into the current instance.
    public void LoadData(GameData data)
    {
        if (data == null || data.inventorySlots == null)
        {
            return;
        }

        foreach (var slot in itemSlots)
        {
            slot.itemSO = null;
            slot.quantity = 0;
            slot.UpdateUI();
        }

        for (int i = 0; i < data.inventorySlots.Count && i < itemSlots.Length; i++)
        {
            InventorySlotData savedSlot = data.inventorySlots[i];

            ItemSO item = GetItemByID(savedSlot.itemID);

            if (item == null)
            {
                Debug.LogWarning($"Could not find ItemSO with ID {savedSlot.itemID}");
                continue;
            }

            itemSlots[i].itemSO = item;
            itemSlots[i].quantity = savedSlot.quantity;
            itemSlots[i].UpdateUI();
        }
    }
}
