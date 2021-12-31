using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    //Information on what the crop grows into
    SeedData seedToGrow;

    [Header("Stages of life")]

    public GameObject seed; 
    private GameObject seedling; 
    private GameObject harvestable;
    
    //the growth points of the crop
    int growth;
    
    //How many growth points it takes before it becomes harvestable
    int maxGrowth;
    public enum CropState{
        Seed, Seedling, Harvestable
    }

    //The current stage in the crops growth
    public CropState cropState;

    //Initilisation for crop game object
    //Called when the player plants a seed
    public void Plant(SeedData seedToGrow){

        //Save the seed information
        this.seedToGrow = seedToGrow;

        //Instanstiate the seedling and harvestable gameobjects
        seedling = Instantiate(seedToGrow.seedling, transform);

        //Access the crop item Data
        ItemData cropToYield = seedToGrow.cropToYield;

        //Instantiate the harvestable crop
        harvestable = Instantiate(cropToYield.gameModel, transform);

        //Convert days to grow into minutes
        int hoursToGrow = GameTimestamp.DaysToHours(seedToGrow.daysToGrow);
        //Convert it to minutes
        maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);

        //Set the initial state to seed
        SwitchState(CropState.Seed);
    }
    
    //The crop will grow when watered
    public void Grow(){

        //Increase growthpoint by 1
        growth++;

        //The seed will sprout into a seedling when the growth reaches 50%
        if(growth >= maxGrowth / 2 && cropState == CropState.Seed){
            SwitchState(CropState.Seedling);
        }

        //Check if fully grown then change crop state
        //Grow from seedling to harvestable
        if(growth >= maxGrowth && cropState == CropState.Seedling){
            SwitchState(CropState.Harvestable);
        }

    }

    //Function handles the crop state changes
    public void SwitchState(CropState stateToSwitch){
        //Reset everything and set all GameObjects to inactive
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);

        switch(stateToSwitch){
            //Enable the seed game object
            case CropState.Seed:
                seed.SetActive(true);
                break;
            //Enable the seedling game object
            case CropState.Seedling:
                seedling.SetActive(true);
                break;
            //Enable the harvestable game object
            case CropState.Harvestable:
                harvestable.SetActive(true);
                //Unparent the crop before harvest
                harvestable.transform.parent = null;

                Destroy(gameObject);
                break;
        }

        //Set the current crop state to the state were stiwtching to
        cropState = stateToSwitch;
    }
}
