using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI TextMeshProUGUI;

    [HideInInspector] public ItemDetails ItemDetails;
    [HideInInspector] public int itemQuantity;
}
