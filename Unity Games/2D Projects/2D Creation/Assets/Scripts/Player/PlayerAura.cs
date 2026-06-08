using UnityEngine;

public class PlayerAura : MonoBehaviour, IDataPersistence
{
    private float scaleIncrease = 0.0f;

    private void OnEnable()
    {
        InventoryManager.OnAuraIncreased += IncreaseAura;
    }

    private void OnDisable()
    {
        InventoryManager.OnAuraIncreased -= IncreaseAura;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.localScale = new Vector3(scaleIncrease, scaleIncrease, scaleIncrease);
    }

    // Increase the size of the aura based on awarded amount;
    public void IncreaseAura(float auraAmount)
    {
        scaleIncrease += auraAmount;
        gameObject.transform.localScale = new Vector3(scaleIncrease, scaleIncrease, scaleIncrease);
    }

    public void SaveData(ref GameData data)
    {
        data.auraScale = this.scaleIncrease;
    }

    public void LoadData(GameData data)
    {
        scaleIncrease = data.auraScale;
    }
}
