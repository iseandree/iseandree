using UnityEngine;

public class PlayerAura : MonoBehaviour, IDataPersistence
{
    private float scaleIncrease;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void SaveData(ref GameData data)
    {
        data.auraScale = this.transform.localScale;
    }

    public void LoadData(GameData data)
    {
        this.transform.localScale = data.auraScale;
    }
}
