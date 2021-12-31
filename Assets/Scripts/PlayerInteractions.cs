using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    PlayerController playerController;

    //the land the player is currently selecting
    Land selectedLand = null;

    //The Interactable object the player is currently selecting
    InteractableObject selectedInteractable = null;



    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down,out hit, 1)){
            OnInteractibleHit(hit);
        }
    }

    //Handles what happenswhen the raycast hits somthing
    void OnInteractibleHit(RaycastHit hit){
        Collider other = hit.collider;
        
        if(other.tag == "Land"){
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            return;
        }

        //check if the player is going to interact with an item
        if(other.tag == "Item"){
            //Set the interactable to the currently selected interactable
            selectedInteractable = other.GetComponent<InteractableObject>();
            return;
        }

        //Deselect the interactable if the player is not standing on anything at the moment.
        if(selectedInteractable != null){
            selectedInteractable = null;
        }

        if(selectedLand != null){
            selectedLand.Select(false);
            selectedLand = null;
        }
    }

    //handles the slection process of the land
    void SelectLand(Land land){

        //Set the previously selected land to false(if any)
        if(selectedLand != null){
            selectedLand.Select(false);
        }
        //Set the new selected land to the land we are stood on
        selectedLand = land;
        land.Select(true);
    }

    //Triggered when player presses tool button key
    public void Interact(){
        //The player shoiuldnt be able to use a tool when items in hand
        if(InventoryManager.Instance.equippedItem != null){
            return;
        }
        //check to see if player is on interactable land
        if(selectedLand != null){
            selectedLand.Interact();
            return;
        }

        Debug.Log("Not on any land");
    }

    //Triggered when player presses the item interaction button
    public void ItemInteract(){
        //if the player is holding somthing, keep it in his inventory
        if(InventoryManager.Instance.equippedItem != null){
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
            return;
        }

        

        //If the player isnt holding anything pick up an item
        //Check if there is a interactable selected
        if(selectedInteractable != null){
            //pick it up
            selectedInteractable.Pickup();
        }
    }
   
    
}
