using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Camera mainCamera;
    private Canvas parentCanvas;
    private Transform parentItem;
    private GameObject draggedItem;
    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI TextMeshProUGUI;
    

    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;
    [HideInInspector] public bool isSelected = false;
    [SerializeField] private UIInventoryBar inventoryBar = null;
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] private GameObject inventoryTextBoxPrefab = null;
    [SerializeField] private int slotNumber = 0;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }
    private void Start()
    {
        mainCamera = Camera.main;
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            //Disable keyboard input
            Player.Instance.DisablePlayerInputAndResetMovement();

            //Instantiate gameObject as dragged item
            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);

            // Get image for dragged item
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;

            SetSelectedItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move game object as dragged item
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Destroy game object as dragged item
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            // If drag ends over inventory bar, get item drag is over and swap them
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {
                // get the slot number where the drag ended
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().slotNumber;

                // Swap inventory items in inventory list
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);

                DestroyInventoryTextBox();

                // Clear the selected item
                ClearSelectedItem();

            }
            //else attempt to drop the item if it can be dropped
            else
            {
                if(itemDetails.canBeDropped)
                {
                    DropSelectedItemAtMousePosition();
                }
            }
            Player.Instance.EnablePlayerInput();
        }
    }


    /// <summary>
    /// Sets this inventory slot item to be selected
    /// </summary>
    private void SetSelectedItem()
    {
        // Clear currently highlighted items
        inventoryBar.ClearHighlightOnInventorySlots();

        // Highlight item on inventory bar
        isSelected = true;

        // Set highlighted inventory slots
        inventoryBar.SetHighlightedInventorySlots();

        // Set item selected in inventory
        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, itemDetails.itemCode);
    }


    private void ClearSelectedItem()
    {
        // Clear currently highlighted item
        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = false;

        // Set no item selected in inventory
        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
    }



    /// <summary>
    /// Drops the item (if selected) at the current mouse positon. Called by the DropItem event
    /// </summary>
    private void DropSelectedItemAtMousePosition()
    {
        if (itemDetails != null && isSelected)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            // Create item from prefab at mouse position
            GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = itemDetails.itemCode;

            // Remove item from players inventory
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);

            // if no more of the item then clear selected
            if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) == -1)
            {
                ClearSelectedItem();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // if left click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // If inventory slot currently selected then deselect
            if (isSelected == true)
            {
                ClearSelectedItem();
            }
            else
            {
                if (itemQuantity > 0)
                {
                    SetSelectedItem();
                }
            }
        }
    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Populate text bax with item details
        if (itemQuantity != 0)
        {
            // Instantiate inventory text box
            inventoryBar.inventoryTextBoxGameobject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextBoxGameobject.transform.SetParent(parentCanvas.transform, false);

            UIInventoryTextBox inventoryTextBox = inventoryBar.inventoryTextBoxGameobject.GetComponent<UIInventoryTextBox>();

            // Set item type description
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            // Populate text box
            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            // Set textbox position according to the inventory bar position
            if (inventoryBar.isInventoryBarPositionBottom)
            {
                inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y +50f,transform.position.z);
            }
            else
            {
                inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextBox();
    }

    public void DestroyInventoryTextBox()
    {
        if (inventoryBar.inventoryTextBoxGameobject != null) 
        {
            Destroy(inventoryBar.inventoryTextBoxGameobject);
        }
    }

   
}