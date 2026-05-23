using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

}

[System.Serializable]
public class DialogueLine
{
    public ActorSO speaker;
    [TextArea(3, 5)] public string text;    // Sets the size of the box based on min and max of TextArea
}

[System.Serializable]
public class DialogueOption
{
    public DialogueSO nextDialogue;
    public string optionText;    
}