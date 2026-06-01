using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Represents a UI slot that displays the reward(s) provided by an objective
public class ObjectiveRewardSlot : MonoBehaviour
{
    // Private UI Variables
    [SerializeField] private Image rewardImage;
    [SerializeField] private TMP_Text rewardQuantity;

    // Updates the UI to show the potential rewards that will be provided by an objective
    public void DisplayReward(Sprite sprite, int quantity)
    {
        rewardImage.sprite = sprite;
        rewardQuantity.text = quantity.ToString();
    }
}
