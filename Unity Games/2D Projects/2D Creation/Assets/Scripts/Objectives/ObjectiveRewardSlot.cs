using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveRewardSlot : MonoBehaviour
{
    public Image rewardImage;
    public TMP_Text rewardQuantity;

    public void DisplayReward(Sprite sprite, int quantity)
    {
        rewardImage.sprite = sprite;
        rewardQuantity.text = quantity.ToString();
    }
}
