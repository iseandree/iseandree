using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* Represents a generic dictionary that can be serialized and deserialized by Unity's serialization system.
 Code sourced/inspired by Shaped by Rain Studios https://www.youtube.com/watch?v=aUi9aijvpgs */
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // Private Variables - Lists for dictionaries
    [SerializeField] private List<TKey> keys = new List<TKey>();    // For keys, respectively
    [SerializeField] private List<TValue> values = new List<TValue>();  // For values, respectively

    // Invoked before the object is serialized to save the dictionary to the lists
    public void OnBeforeSerialize()
    {
        // Clear keys and values
        keys.Clear();
        values.Clear();

        // Copy everything from the dictionary into the lists
        foreach(KeyValuePair<TKey,TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // Invoked after the object is serialized to load the dictionary from the lists
    public void OnAfterDeserialize()
    {
        // Make sure the dictionary is cleared
        this.Clear();

        // Make sure the keys match the values
        if(keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize the SD but the amount of keys : " + keys.Count 
                + " don't match the number of values : " + values.Count);
        }

        // Loop through and add each key value pair
        for(int i = 0; i<keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
