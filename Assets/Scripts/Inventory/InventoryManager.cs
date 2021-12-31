using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance {get; private set;}

    //If there is more than one instance running destroy it
    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(this);
        } else {
            //set the static instance to this instance
            Instance = this;
        }
    }


    [Header("Tools")]
    //The set of tools
    public ItemData[] tools = new ItemData[8];

    //The tool in the players hand
    public ItemData equippedTool = null;

    [Header("Items")]
    //item Slots
    public ItemData[] items = new ItemData[8];

    //The item in the players hand
    public ItemData equippedItem = null;

    //The transform for the player to hold items
    public Transform handPoint;

    //Equipping
    
    //Handles movement of item from inventory to hand
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType){

        if(inventoryType == InventorySlot.InventoryType.Item){
        //cACHE THE INVENTORY SLOT itemdata from inventory manager
        ItemData itemToEquip = items[slotIndex];
        
        //Change the inventory slot to the hands
        items[slotIndex] = equippedItem;

        //Chang ethe hand slot to the inventory slots
        equippedItem = itemToEquip;

        //Update the changes in the scene
        RenderHand();
        } else {
        //cACHE THE INVENTORY SLOT itemdata from inventory manager
        ItemData toolToEquip = tools[slotIndex];
        
        //Change the inventory slot to the hands
        tools[slotIndex] = equippedTool;

        //Chang ethe hand slot to the inventory slots
        equippedTool = toolToEquip;
        }

        UIManager.Instance.RenderInventory();
    }

    //Handles movement of item from hand to inventory
    public void HandToInventory(InventorySlot.InventoryType inventoryType){
        if(inventoryType == InventorySlot.InventoryType.Item){
            //iterate through each inventory slot and find an empty slot
            for(int i =0; i < items.Length; i++){
                if(items[i] == null){
                    //send the equpped item to empty slot
                    items[i] = equippedItem;
                    //Remove the item from the hand
                    equippedItem = null;
                    break;
                }
            }
             RenderHand();
        } else {
            for(int i =0; i < tools.Length; i++){
                if(tools[i] == null){
                    //send the equpped item to empty slot
                    tools[i] = equippedTool;
                    //Remove the item from the hand
                    equippedTool = null;
                    break;
                }
            }
        }   
        //Update the UI
        UIManager.Instance.RenderInventory();
    }

    //Render the players equiped item in the scene
    public void RenderHand(){
        //Reset objects on the hand
        if(handPoint.childCount > 0){
            Destroy(handPoint.GetChild(0).gameObject);
        }

        //Check to see if the player is holding anything
        if(equippedItem != null){
            //Instantiate the game model to the players hand
            Instantiate(equippedItem.gameModel, handPoint);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
