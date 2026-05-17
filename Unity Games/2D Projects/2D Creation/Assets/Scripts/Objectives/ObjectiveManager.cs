using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour, IDataPersistence
{

    public InventorySlot[] itemSlots;
    public int food;
    public TMP_Text foodText;
    public int water;
    public TMP_Text waterText;


    private void OnEnable()
    {
        Objective.OnObjectiveAccepted += AddQuestObjective;
    }
    private void OnDisable()
    {
        Objective.OnObjectiveAccepted -= AddQuestObjective;
    }

    private void Start()
    {
        
    }

    private void AddQuestObjective(GameObjectiveScriptableObject objectiveSO, bool isAccepted)
    {
        
    }

    public void SaveData(ref GameData data)
    {
        
    }

    public void LoadData(GameData data)
    {
        
    }
}
