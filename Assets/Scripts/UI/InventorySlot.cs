using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    ItemData itemToDisplay;
    
    public Image itemDisplayImage;

    public enum InventoryType{
        Item, Tool
    }

    //Determines which inventory slot the tool is in
    public InventoryType inventoryType;
    int slotIndex;
    public void Display(ItemData itemToDisplay){
        //Check if there is an item to display
        if(itemToDisplay != null){

            //Switch the thumbnail over
            itemDisplayImage.sprite = itemToDisplay.thumbnail;
            this.itemToDisplay = itemToDisplay;

            itemDisplayImage.gameObject.SetActive(true);
            return;
        }

        itemDisplayImage.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData){

        //Move item from inventory to hand
        InventoryManager.Instance.InventoryToHand(slotIndex, inventoryType);
    }


    //Sets the slot index 
    public void AssignIndex(int slotIndex){
        this.slotIndex = slotIndex;
    }

    //Display the item info on the infobox when the player mouses over
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(itemToDisplay);
    }

    //Reset the item info on the infobox when the player mouses over
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }
}
