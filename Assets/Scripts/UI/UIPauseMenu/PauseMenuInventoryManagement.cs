using System.Collections.Generic;
using UnityEngine;

public class PauseMenuInventoryManagement : MonoBehaviour
{
    [SerializeField] private PauseMenuInventoryManagementSlot[] inventoryManagementSlots = null;

    public GameObject inventoryManagementDraggedItemPrefab;

    [SerializeField] private Sprite transparent16x16 = null;

    [HideInInspector] public GameObject inventoryTextBoxGameobject;

    private void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += PopulatePlayerInventory;

        // Populate player inventory
        if (InventoryManager.Instance != null)
        {
            PopulatePlayerInventory(InventoryLocation.player, InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player]);
        }
    }

    private void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= PopulatePlayerInventory;
        DestroyInventoryTextBoxGameobject();
    }

    public void DestroyInventoryTextBoxGameobject()
    {
        // Destroy inventory text box if created
        if (inventoryTextBoxGameobject != null)
        {
            Destroy(inventoryTextBoxGameobject);
        }
    }

    public void DestroyCurrentlyDraggedItems()
    {
        // loop through all player invnetory items
        for (int i = 0; i< InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player].Count; i++)
        {
            if (inventoryManagementSlots[i].draggedItem != null)
            {
                Destroy(inventoryManagementSlots[i].draggedItem);
            }
        }
    }

    private void PopulatePlayerInventory(InventoryLocation inventoryLocation, List<InventoryItem> playerInventoryList)
    {
        if (inventoryLocation == InventoryLocation.player)
        {
            InitialiseInventoryManagementSlots();

            // loop through all player inventory items
            for (int i = 0; i < InventoryManager.Instance.inventoryLists[(int)(InventoryLocation.player)].Count; i++)
            {
                // Get inventory item details
                inventoryManagementSlots[i].itemDetails = InventoryManager.Instance.GetItemDetails(playerInventoryList[i].itemCode);
                inventoryManagementSlots[i].itemQuantity = playerInventoryList[i].itemQuantity;

                if (inventoryManagementSlots[i].itemDetails != null)
                {
                    // update inventory management slot with image and quantity
                    inventoryManagementSlots[i].inventoryManagementSlotImage.sprite = inventoryManagementSlots[i].itemDetails.itemSprite;
                    inventoryManagementSlots[i].textMeshProUGUI.text = inventoryManagementSlots[i].itemQuantity.ToString();
                }
            }
        }
    }

    private void InitialiseInventoryManagementSlots()
    {
        // Clear inventory slots
        for (int i = 0; i< Settings.playerMaximumInventoryCapacity; i++)
        {
            inventoryManagementSlots[i].greyedOutImageGO.SetActive(false);
            inventoryManagementSlots[i].itemDetails = null;
            inventoryManagementSlots[i].itemQuantity = 0;
            inventoryManagementSlots[i].inventoryManagementSlotImage.sprite = transparent16x16;
            inventoryManagementSlots[i].textMeshProUGUI.text = "";
        }

        // Grey out unavailable slots
        for (int i = InventoryManager.Instance.inventoryListCapacityIntArray[(int)InventoryLocation.player]; i < Settings.playerMaximumInventoryCapacity; i++)
        {
            inventoryManagementSlots[i].greyedOutImageGO.SetActive(true);
        }
    }
}
