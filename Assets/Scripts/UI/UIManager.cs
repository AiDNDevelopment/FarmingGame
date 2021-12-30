using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{

    public static UIManager Instance {get; private set;}

    [Header("Status Bar")]
    //Tool equip slot on the status bar
    public Image toolEquipSlot;

    //TimeUI
    public Text timeText; 
    public Text dateText;

    [Header("Inventory System")]
    public GameObject inventoryPanel;

    //The tool hand slot on the inventory panel
    public HandInventorySlot toolHandSlot;
    //The tool slot UIs
    public InventorySlot[] toolSlots;

    //The item hand slot on the invnetory panel
    public HandInventorySlot itemHandSlot;
    //The item slot UIs
    public InventorySlot[] itemSlots;
    //item info box
    public Text itemNameText;
    public Text itemDescriptionText;

    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start(){
        RenderInventory();
        AssignSlotIndexes();

        //Add the UIManager to a list of objects TimeManger will notify when the time updates
        TimeManager.Instance.RegisterTracker(this);
    }

    //iterates through the slot ui elements and assign it its reference slot index
    public void AssignSlotIndexes(){
        for(int i =0; i<toolSlots.Length; i++){
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);

        }
    }

    //Render the inventory screen to reflect players inventory
    public void RenderInventory(){
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

        //render the tool selection
        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        //render the item selection
        RenderInventoryPanel(inventoryItemSlots, itemSlots);

        //Render the equipped slots
        toolHandSlot.Display(InventoryManager.Instance.equippedTool);
        itemHandSlot.Display(InventoryManager.Instance.equippedItem);

        //Get equipped tool from inventory manager
        ItemData equippedTool = InventoryManager.Instance.equippedTool;
        ItemData equippedItem = InventoryManager.Instance.equippedItem;

        //check if there is an item to display
        if(equippedTool != null){

            //Switch the thumbnail over
            toolEquipSlot.sprite = equippedTool.thumbnail;
            
            toolEquipSlot.gameObject.SetActive(true);
            return;
        }

        toolEquipSlot.gameObject.SetActive(false);

    }

    void RenderInventoryPanel(ItemData[] slots, InventorySlot[] uiSlots)
    {
        for(int i=0; i < uiSlots.Length; i++){
            uiSlots[i].Display(slots[i]);
        }        
    }

    public void ToggleInventoryPanel(){

        //If the panel is hidden show it and vice versa
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        RenderInventory();
    }   

    //Display item info in the infobox
    public void DisplayItemInfo(ItemData data){
        if(data == null){
            itemNameText.text = "";
            itemDescriptionText.text ="";

            return;
        }

        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;

    }

    //Callback to handle the UI for time
    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Handle the time
        int hours = timestamp.hour;
        int minutes = timestamp.minute;
    
        //AM or PM
        string prefix = "AM: ";

        //Convert hours to 12 hour clock 
        if(hours > 12){
            //Time Becomes PM
            prefix ="PM: ";
            hours -= 12;
        }

        //Format it for the time text display
        timeText.text = prefix + hours + ":" + minutes.ToString("00");

        //Handle the date
        int day = timestamp.day; 
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        //Format it for the date text display
        dateText.text = season + " " + day + " ("+ dayOfTheWeek + ")";
    }
}
