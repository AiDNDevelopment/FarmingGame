using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    //Information on what the crop grows into
    SeedData seedToGrow;

    [Header("Stages of life")]

    public GameObject seed; 
    public GameObject wilted;
    private GameObject seedling; 
    private GameObject harvestable;
    //the growth points of the crop
    int growth;
    
    //How many growth points it takes before it becomes harvestable
    int maxGrowth;

    //Crops die after 48 hours in game time without water before it dies 
    int plantMaxHealth = GameTimestamp.HoursToMinutes(48);
    int plantHealth;
    public enum CropState{
        Seed, Seedling, Harvestable, Wilted
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

        //Check to see if plant is regrowable
        if(seedToGrow.regrowable){
            //Get the RegrowableHarvestBehaviour from the GameObject
            RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();

            //Initialise the harvestable
            regrowableHarvest.SetParent(this);
        }

        //Set the initial state to seed
        SwitchState(CropState.Seed);
    }
    
    //The crop will grow when watered
    public void Grow(){

        //Increase growthpoint by 1
        growth++;

        //Restore the health of the plant when it is watered and grows
        if(plantHealth < plantMaxHealth){
            plantHealth++;
        }

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

    //The crop will progressively wither when the ground is dry
    public void Wither(){
        plantHealth--;
        //If health is below 0 and the crop has germinated, then we kill it
        if(plantHealth <= 0 && cropState != CropState.Seed){
            SwitchState(CropState.Wilted);
        }

    }

    //Function handles the crop state changes
    public void SwitchState(CropState stateToSwitch){
        //Reset everything and set all GameObjects to inactive
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);

        switch(stateToSwitch){
            //Enable the seed game object
            case CropState.Seed:
                seed.SetActive(true);
                break;
            //Enable the seedling game object
            case CropState.Seedling:
                seedling.SetActive(true);
                plantHealth = plantMaxHealth;
                break;
            //Enable the harvestable game object
            case CropState.Harvestable:
                harvestable.SetActive(true);

                //if the seed is not regrowable, detach the harvestable from this crop GameObject and destroys it
                if(!seedToGrow.regrowable){
                    //Unparent the crop before harvest
                    harvestable.transform.parent = null;
                    Destroy(gameObject);
                }
                break;

            case CropState.Wilted:
                wilted.SetActive(true);
                break;
        }

        //Set the current crop state to the state were stiwtching to
        cropState = stateToSwitch;
    }

    public void Regrow(){
        //Reset growth
        //Get the regrowth time in hours
        int hoursToRegrow = GameTimestamp.DaysToHours(seedToGrow.daysToRegrow);
        growth = maxGrowth - GameTimestamp.HoursToMinutes(hoursToRegrow);
        //Change the crop state back to seedling
        SwitchState(CropState.Seedling);
    }
}
