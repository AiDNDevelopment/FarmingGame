using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public enum LandStatus{
        Soil, Farmland, Watered
    }

    public LandStatus landStatus;
    public Material soilMat, farmland, wateredMat;
    new Renderer renderer;


    //If Select set the game object true
    public GameObject select;

    //Cache the time the land was watered
    GameTimestamp timeWatered;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        SwitchLandStatus(LandStatus.Soil);
        //Disables the selection box cus i cba enable/disableing it everytime i wanna work on it
        Select(false);

        //Add this to the timemanagers listener list
        TimeManager.Instance.RegisterTracker(this);
    }

    public void SwitchLandStatus(LandStatus statusToSwitch){
        landStatus = statusToSwitch;
        Material materialToSwitch = soilMat;
        //decides what material to switch to
        switch(statusToSwitch){
            case LandStatus.Soil:
            //Switch to soil
            materialToSwitch = soilMat;
            break;
            case LandStatus.Farmland:
            //switch to farmland
            materialToSwitch = farmland;
            break;
            case LandStatus.Watered:
            //switch to watered
            materialToSwitch = wateredMat;

            //Cache the time it was watered
            timeWatered = TimeManager.Instance.GetGameTimestamp();
            break;
        }

        //render to apply changes
        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle){
        select.SetActive(toggle);
    }

//When the player presses the interact button
    public void Interact(){
        //Check the players tool slot
        ItemData toolSlot = InventoryManager.Instance.equippedTool;

        //try casting the itemdata in the toolslot as equipment data
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        //check if it of the type equipment data
        if(equipmentTool != null){
            //get
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch(toolType){
                case EquipmentData.ToolType.Hoe:
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    SwitchLandStatus(LandStatus.Watered);
                    break;
            }
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Checked if 24 hours has passed since last watered
        if(landStatus == LandStatus.Watered){

            //hours since the land was last watered
            int hoursElapsed = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log(hoursElapsed + " Hours since this ground was last watered");

            if(hoursElapsed > 24){
                //Dry that ground up
                SwitchLandStatus(LandStatus.Farmland);
            }
        }
    }
}
